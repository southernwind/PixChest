using GenJsonConfig.Attributes;
using MediaDeck.Composition.Constants;

namespace MediaDeck.Composition.Stores.Config.Model;

[Inject(InjectServiceLifetime.Singleton)]
[GenerateJsonConfigDto]
public class PathConfigModel {
	/// <summary>
	/// サムネイルフォルダパス
	/// </summary>

	public ReactiveProperty<string> ThumbnailFolderPath {
		get;
	} = new(FilePathConstants.ThumbnailDirectoryPath);

	/// <summary>
	/// 一時フォルダパス
	/// </summary>

	public ReactiveProperty<string> TemporaryFolderPath {
		get;
	} = new(Path.Combine(FilePathConstants.BaseDirectory, "temp"));

	/// <summary>
	/// FFMpegフォルダパス
	/// </summary>

	public ReactiveProperty<string> FFMpegFolderPath {
		get;
	} = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets"));
}