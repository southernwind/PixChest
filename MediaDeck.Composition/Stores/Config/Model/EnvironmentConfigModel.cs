using GenJsonConfig.Attributes;
using MediaDeck.Composition.Enum;

namespace MediaDeck.Composition.Stores.Config.Model;

[Inject(InjectServiceLifetime.Singleton)]
[GenerateJsonConfigDto]
public class EnvironmentConfigModel {
	/// <summary>
	/// アプリケーションのテーマ設定
	/// </summary>
	public ReactiveProperty<AppTheme> Theme {
		get;
	} = new(AppTheme.Default);

	/// <summary>
	/// アプリケーションのシステムバックドロップ設定
	/// </summary>
	public ReactiveProperty<AppSystemBackdrop> SystemBackdrop {
		get;
	} = new(AppSystemBackdrop.Mica);
}