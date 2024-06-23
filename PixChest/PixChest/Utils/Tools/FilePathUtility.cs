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
}
