using R3.JsonConfig.Attributes;

namespace MediaDeck.Composition.Stores.State.Model;

/// <summary>
/// 新規タブのデフォルト状態
/// </summary>
[Inject(InjectServiceLifetime.Singleton)]
[GenerateR3JsonConfigDto]
public class DefaultTabStateModel(SearchStateModel searchState, ViewerStateModel viewerState) {
	public SearchStateModel SearchState {
		get;
		set;
	} = searchState;

	public ViewerStateModel ViewerState {
		get;
		set;
	} = viewerState;

	/// <summary>
	/// 現在選択されているリポジトリの種類
	/// </summary>
	public ReactiveProperty<RepositoryType> ActiveRepository {
		get;
		set;
	} = new(RepositoryType.Folder);
}