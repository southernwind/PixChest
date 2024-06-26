using System.ComponentModel;

namespace PixChest.Utils.Enums;
/// <summary>
/// メディアタイプ
/// </summary>
public enum MediaType {
	/// <summary>
	/// 画像
	/// </summary>
	[Description("Image")]
	Image,
	/// <summary>
	/// 動画
	/// </summary>
	[Description("Video")]
	Video
}
