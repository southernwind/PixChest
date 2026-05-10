using GenJsonConfig.Attributes;
using MediaDeck.Composition.Enum;

namespace MediaDeck.Composition.Stores.State.Model;

/// <summary>
/// アプリケーション全体で共有される状態（Singleton）
/// </summary>
[Inject(InjectServiceLifetime.Singleton)]
[GenerateJsonConfigDto]
public class AppStateModel(DefaultTabStateModel defaultTabState) {

	/// <summary>
	/// アプリケーションのテーマ
	/// </summary>
	public ReactiveProperty<AppTheme> Theme {
		get;
		set;
	} = new(AppTheme.Default);

	/// <summary>
	/// 新規タブのデフォルト状態
	/// </summary>
	public DefaultTabStateModel DefaultTabState {
		get;
		set;
	} = defaultTabState;
}