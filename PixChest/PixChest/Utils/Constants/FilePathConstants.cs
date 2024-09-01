using System.IO;

namespace PixChest.Utils.Constants;
public static class FilePathConstants {
	public static string NoThumbnailFilePath {
		get;
	} = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Assets", "thumbnail_creation_failed.png");
}
