using System.Diagnostics;
using MediaDeck.Common.Base;
using MediaDeck.Common.Utilities;
using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;
using MediaDeck.Composition.Interfaces.Notifications;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Core.Models.Files.Loaders;

namespace MediaDeck.Core.Models.Files;

[Inject(InjectServiceLifetime.Scoped)]
public class MediaContentLibrary : ModelBase {
	private readonly AsyncLock _asyncLock = new();
	private readonly FilesLoader _filesLoader;
	private readonly SearchConfigModel _searchConfig;
	private readonly SearchConditionManager _searchConditionManager;
	/// <summary>コンストラクタ</summary>
	public MediaContentLibrary(FilesLoader filesLoader, SearchConfigModel searchConfig, SearchConditionManager searchConditionManager, ISearchConditionNotificationDispatcher dispatcher) {
		this._filesLoader = filesLoader;
		this._searchConfig = searchConfig;
		this._searchConditionManager = searchConditionManager;

		// Dispatcher の統合ストリームを監視する。
		// Switch により、新しい検索リクエストが来たら前の検索タスクを自動キャンセルする。
		dispatcher.SearchRequested
			.SubscribeAwait(async (_, ct) => {
				await this.SearchAsync(ct).ConfigureAwait(false);
			}, AwaitOperation.Switch, false)
			.AddTo(this.CompositeDisposable);
	}

	/// <summary>検索結果ファイルリスト</summary>
	public ObservableList<IMediaItemModel> Files { get; } = [];

	/// <summary>現在の検索ワード（トークン）条件リスト</summary>
	public ObservableList<ISearchCondition> SearchConditions {
		get {
			return this._searchConditionManager.SearchConditions;
		}
	}

	/// <summary>検索条件候補リスト（サジェスト用）</summary>
	public ObservableList<ISearchCondition> SearchConditionCandidates {
		get {
			return this._searchConditionManager.SearchConditionCandidates;
		}
	}

	/// <summary>最後の検索にかかった時間（ミリ秒）</summary>
	public ReactiveProperty<long?> SearchElapsedMilliseconds { get; } = new();

	/// <summary>追加読み込みが可能かどうか</summary>
	public ReactiveProperty<bool> CanLoadMore { get; } = new(false);

	/// <summary>全件数（上限に達した場合のみ取得）</summary>
	public ReactiveProperty<int?> TotalCount { get; } = new();

	/// <summary>
	/// 検索を実行する。Switch により呼び出し元のキャンセルトークンが連携されるため、
	/// 古い検索タスクは自動的にキャンセルされる。
	/// </summary>
	private async ValueTask SearchAsync(CancellationToken token) {
		await this.LoadInternalAsync(true, token).ConfigureAwait(false);
	}

	/// <summary>
	/// 追加の検索を実行する。
	/// </summary>
	public async ValueTask LoadMoreAsync(CancellationToken token) {
		if (!this.CanLoadMore.Value) {
			return;
		}
		await this.LoadInternalAsync(false, token).ConfigureAwait(false);
	}

	private async ValueTask LoadInternalAsync(bool isInitial, CancellationToken token) {
		using var _ = await this._asyncLock.LockAsync(token).ConfigureAwait(false);
		this.SearchElapsedMilliseconds.Value = null;
		if (isInitial) {
			this.TotalCount.Value = null;
			this.ClearFiles();
		}
		var batch = new List<IMediaItemModel>();
		try {
			await Task.Run(async () => {
				var stopwatch = Stopwatch.StartNew();

				var initialLoadCount = this._searchConfig.InitialLoadCount.Value;
				var incrementalLoadCount = this._searchConfig.IncrementalLoadCount.Value;
				var maxLoadCount = this._searchConfig.MaxLoadCount.Value;

				var skip = isInitial ? 0 : this.Files.Count;
				var stream = this._filesLoader.GetFilesStreamAsync(this.SearchConditions, skip, maxLoadCount, token);

				var totalLoaded = 0;
				var batchLimit = isInitial ? initialLoadCount : incrementalLoadCount;

				await foreach (var fileModel in stream.WithCancellation(token)) {
					batch.Add(fileModel.AddTo(this.CompositeDisposable));
					totalLoaded++;

					if (batch.Count >= batchLimit) {
						this.Files.AddRange(batch);
						batch.Clear();
						batchLimit = incrementalLoadCount;
					}
				}

				if (batch.Count > 0) {
					this.Files.AddRange(batch);
					batch.Clear();
				}

				if (isInitial) {
					if (totalLoaded == maxLoadCount) {
						this.TotalCount.Value = await this._filesLoader.GetTotalCountAsync(this.SearchConditions, token).ConfigureAwait(false);
					} else {
						this.TotalCount.Value = this.Files.Count;
					}
				}

				// 全体件数が現在の表示件数より多ければ、まだ続きがある
				this.CanLoadMore.Value = this.TotalCount.Value > this.Files.Count;

				stopwatch.Stop();
				this.SearchElapsedMilliseconds.Value = stopwatch.ElapsedMilliseconds;

			}, token).ConfigureAwait(false);
		} catch (OperationCanceledException) when (token.IsCancellationRequested) {
			// 新しい検索によりキャンセルされた場合は何もしない
		} finally {
			foreach (var file in batch) {
				file.Dispose();
			}
		}
	}

	/// <summary>ファイルリストをクリアし、各要素を Dispose する。</summary>
	private void ClearFiles() {
		foreach (var file in this.Files) {
			file.Dispose();
		}
		this.Files.Clear();
	}
}