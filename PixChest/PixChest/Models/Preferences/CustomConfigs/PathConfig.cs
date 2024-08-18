using System.IO;

namespace PixChest.Models.Preferences.CustomConfigs;

[AddSingleton]
public class PathConfig : SettingsBase {
	/// <summary>
	/// サムネイルフォルダパス
	/// </summary>

	public SettingsItem<string> ThumbnailFolderPath {
		get;
	} = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "thumbs"));

	/// <summary>
	/// 一時フォルダパス
	/// </summary>

	public SettingsItem<string> TemporaryFolderPath {
		get;
	} = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "temp"));

	/// <summary>
	/// 一時フォルダパス
	/// </summary>

	public SettingsItem<string> FFMpegFolderPath {
		get;
	} = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Assets"));
}
