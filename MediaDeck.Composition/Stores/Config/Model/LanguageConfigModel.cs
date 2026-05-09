using R3.JsonConfig.Attributes;

namespace MediaDeck.Composition.Stores.Config.Model;

[Inject(InjectServiceLifetime.Singleton)]
[GenerateR3JsonConfigDto]
public class LanguageConfigModel {
	/// <summary>
	/// アプリの表示言語タグ (BCP-47)。
	/// 空文字列の場合はシステムの既定言語を使用します。
	/// 例: "ja-JP", "en-US"
	/// </summary>
	public ReactiveProperty<string> Language {
		get;
	} = new(string.Empty);
}