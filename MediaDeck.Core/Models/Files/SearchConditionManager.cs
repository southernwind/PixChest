using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Interfaces.Notifications;
using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Composition.Stores.State.Model;
using MediaDeck.Core.Models.Files.SearchConditions;
using MediaDeck.Core.Models.Repositories;
using R3;

namespace MediaDeck.Core.Models.Files;

[Inject(InjectServiceLifetime.Scoped)]
public class SearchConditionManager : ModelBase {
	private readonly ITagsManager _tagsManager;
	private readonly FolderRepository _folderRepository;

	public SearchConditionManager(ISearchConditionNotificationDispatcher dispatcher, ITagsManager tagsManager, FolderRepository folderRepository, TabStateModel tabState) {
		this._tagsManager = tagsManager;
		this._folderRepository = folderRepository;

		// 検索ワード（トークン）の追加・削除・更新を SearchConditions リストに反映する
		dispatcher.AddRequest.Subscribe(this.SearchConditions.Add).AddTo(this.CompositeDisposable);
		dispatcher.RemoveRequest.Subscribe(x => this.SearchConditions.Remove(x)).AddTo(this.CompositeDisposable);
		dispatcher.UpdateRequest.Subscribe(x => x(this.SearchConditions)).AddTo(this.CompositeDisposable);

		// タブ状態の初期値を SearchConditions に復元する
		this.SearchConditions.AddRange(tabState.SearchState.SearchCondition.ToArray());

		// SearchConditions の変更をタブ状態に同期する
		this.SearchConditions.ObserveChanged()
			.Subscribe(_ => {
				tabState.SearchState.SearchCondition.Clear();
				tabState.SearchState.SearchCondition.AddRange(this.SearchConditions.ToArray());
			})
			.AddTo(this.CompositeDisposable);

		// タグおよびフォルダの変更を監視して、サジェスト候補をリアクティブに更新する
		this._tagsManager.Tags.ObserveChanged()
			.Subscribe(_ => {
				this.UpdateCandidates();
			})
			.AddTo(this.CompositeDisposable);

		this._folderRepository.RootFolder
			.Subscribe(_ => {
				this.UpdateCandidates();
			})
			.AddTo(this.CompositeDisposable);

		// 候補リストを初期化する
		this.UpdateCandidates();
	}

	/// <summary>現在の検索ワード（トークン）条件リスト</summary>
	public ObservableList<ISearchCondition> SearchConditions { get; } = [];

	/// <summary>検索条件候補リスト（サジェスト用）</summary>
	public ObservableList<ISearchCondition> SearchConditionCandidates { get; } = [];

	/// <summary>
	/// 検索条件の候補リストを最新の状態に更新します。
	/// </summary>
	private void UpdateCandidates() {
		this.SearchConditionCandidates.Clear();

		// タグ候補の追加
		this.SearchConditionCandidates.AddRange(
			this._tagsManager.Tags.Select(x => new TagSearchCondition(this._tagsManager) { TagId = x.TagId } as ISearchCondition));

		// フォルダ候補の追加
		this.SearchConditionCandidates.AddRange(
			this._folderRepository.GetAllFolders().Select(x => new FolderSearchCondition { FolderPath = x.FolderPath } as ISearchCondition));

		// MediaItem の各プロパティに対する prop. サジェストスタブを登録する
		this.SearchConditionCandidates.AddRange(
			MediaItemPropertyCatalog.Descriptors.Select(d => new PropertySearchCondition { PropertyName = d.Name } as ISearchCondition));
	}
}