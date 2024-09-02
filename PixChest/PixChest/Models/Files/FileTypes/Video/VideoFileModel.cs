using PixChest.Models.Files.FileTypes.Base;
using PixChest.Models.Files.FileTypes.Image;

namespace PixChest.Models.Files.FileTypes.Video;
public class VideoFileModel(long id, string filePath) : FileModel(id, filePath, _fileOperator) {
	private static readonly ImageFileOperator _fileOperator = new();
}
