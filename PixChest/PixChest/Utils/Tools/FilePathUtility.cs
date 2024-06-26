using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PixChest.Utils.Tools;
internal static class FilePathUtility
{
    /// <summary>
    /// サムネイル相対ファイルパス取得
    /// </summary>
    /// <param name="filePath">生成元ファイルパス</param>
    /// <returns>サムネイル相対ファイルパス</returns>
    public static string GetThumbnailRelativeFilePath(string filePath) {
		var thumbDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "thumbs");
		return $"{Path.Combine(thumbDir, $"{string.Join("", SHA512.HashData(Encoding.UTF8.GetBytes(filePath)).Select(b => $"{b:X2}"))}".Insert(2, @"\"))}.jpg";
	}

	/// <summary>
	/// 指定したファイルパスのファイルが管理対象の拡張子を持っているかどうかを調べる
	/// </summary>
	/// <param name="path">ファイルパス</param>
	/// <param name="targetExtension">対象ファイル拡張子</param>
	/// <returns>管理対象か否か</returns>
	public static bool IsTargetFile(this string path, string[] targetExtension) {
		return targetExtension.Contains(Path.GetExtension(path).ToLower());
	}

	/// <summary>
	/// 指定したファイルパスのファイルが動画拡張子を持っているかどうかを調べる
	/// </summary>
	/// <param name="path">ファイルパス</param>
	/// <param name="videoExtension">動画ファイル拡張子</param>
	/// <returns>動画ファイルか否か</returns>
	public static bool IsVideoFile(this string path, string[] videoExtension) {
		return videoExtension.Contains(Path.GetExtension(path).ToLower()!);
	}
}
