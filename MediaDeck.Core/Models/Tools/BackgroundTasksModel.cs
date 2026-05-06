using MediaDeck.Common.Base;
using MediaDeck.Core.Services.FileStatusUpdator;

namespace MediaDeck.Core.Models.Tools;

/// <summary>
/// バックグラウンドタスク管理Model。
/// </summary>
[Inject(InjectServiceLifetime.Singleton)]
public class BackgroundTasksModel : ModelBase {
	private readonly FileStatusUpdatorService _fileStatusUpdater;
	private readonly IFileHashUpdatorService _updateFileHashBackgroundService;
	private CancellationTokenSource _fileStatusUpdaterCts = new();

	/// <summary>
	/// バックグラウンド実行キュー
	/// </summary>
	public Subject<Func<Task>> Actions {
		get;
	} = new();

	/// <summary>
	/// バックグラウンドタスク項目一覧
	/// </summary>
	public IReadOnlyList<BackgroundTaskStatusItemModel> TaskItems {
		get;
	}

	/// <summary>
	/// バックグラウンドタスク管理Modelを初期化する。
	/// </summary>
	/// <param name="fileStatusUpdater">ファイル状態更新サービス</param>
	/// <param name="updateFileHashBackgroundService">ファイルハッシュ更新サービス</param>
	public BackgroundTasksModel(FileStatusUpdatorService fileStatusUpdater, IFileHashUpdatorService updateFileHashBackgroundService) {
		this._fileStatusUpdater = fileStatusUpdater;
		this._updateFileHashBackgroundService = updateFileHashBackgroundService;

		this.TaskItems = [
			new BackgroundTaskStatusItemModel(
				"Update file status",
				this._fileStatusUpdater.CompletedCount,
				this._fileStatusUpdater.TargetCount,
				() => {
					this._fileStatusUpdaterCts.Cancel();
					this._fileStatusUpdaterCts.Dispose();
					this._fileStatusUpdaterCts = new();
					this.Actions.OnNext(() => this._fileStatusUpdater.UpdateFileInfo(this._fileStatusUpdaterCts.Token));
				},
				() => {
					this._fileStatusUpdaterCts.Cancel();
					this._fileStatusUpdater.TargetCount.Value = 0;
					this._fileStatusUpdater.CompletedCount.Value = 0;
				}),
			new BackgroundTaskStatusItemModel(
				"Update file hash",
				this._updateFileHashBackgroundService.CompletedCount,
				this._updateFileHashBackgroundService.TargetCount,
				() => this.Actions.OnNext(() => this._updateFileHashBackgroundService.EnqueueAllHashUpdatesAsync()),
				() => this._updateFileHashBackgroundService.CancelUpdate()),
			new BackgroundTaskStatusItemModel(
				"Update full hash",
				this._updateFileHashBackgroundService.FullHashCompletedCount,
				this._updateFileHashBackgroundService.FullHashTargetCount,
				() => this.Actions.OnNext(() => this._updateFileHashBackgroundService.CheckAndEnqueueFullHashUpdatesAsync()),
				() => this._updateFileHashBackgroundService.CancelFullHashUpdate()),
		];

		this.Actions.Synchronize()
			.ObserveOnThreadPool()
			.SubscribeAwait(async (action, ct) => await action().ConfigureAwait(false), AwaitOperation.Sequential, false)
			.AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// バックグラウンドタスクを開始する。
	/// </summary>
	public void Start() {
		this._fileStatusUpdaterCts.Cancel();
		this._fileStatusUpdaterCts.Dispose();
		this._fileStatusUpdaterCts = new();
		this.Actions.OnNext(() => this._fileStatusUpdater.UpdateFileInfo(this._fileStatusUpdaterCts.Token));
	}

	/// <summary>
	/// リソースを解放する。
	/// </summary>
	/// <param name="disposing">マネージドリソースを解放するかどうか</param>
	protected override void Dispose(bool disposing) {
		if (disposing) {
			this._fileStatusUpdaterCts.Cancel();
			this._fileStatusUpdaterCts.Dispose();
		}
		base.Dispose(disposing);
	}
}