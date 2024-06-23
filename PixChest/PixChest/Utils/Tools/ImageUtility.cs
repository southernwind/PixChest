using System.IO;

using ImageMagick;

namespace PixChest.Utils.Tools; 
internal static class ImageUtility {
	internal static byte[] CreateThumbnail(Stream imageStream, int width, int height) {
		using var ms = new MemoryStream();
		using var mi = new MagickImage(imageStream);
		mi.AutoOrient();
		mi.Thumbnail(width, height);
		mi.Format = MagickFormat.Jpg;
		mi.Write(ms);
		return ms.ToArray();
	}
}
