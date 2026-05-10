using R3.JsonConfig.Attributes;

namespace MediaDeck.Composition.Stores.Config.Model;

/// <summary>
/// サムネイル設定
/// </summary>
[Inject(InjectServiceLifetime.Singleton)]
[GenerateR3JsonConfigDto]
public class ThumbnailConfigModel {
	/// <summary>
	/// サムネイル作成サイズ
	/// </summary>
	public ReactiveProperty<int> ThumbnailSize {
		get;
	} = new(300);
}