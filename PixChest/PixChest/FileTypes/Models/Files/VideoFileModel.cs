using PixChest.Models.Files.FileTypes.Video;

namespace PixChest.FileTypes.Models.Files;
public class VideoFileModel(long id, string filePath) : FileModel(id, filePath, _fileOperator) {
	private static readonly VideoFileOperator _fileOperator = new();
}
