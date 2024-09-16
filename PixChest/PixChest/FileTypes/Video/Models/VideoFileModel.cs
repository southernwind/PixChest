using PixChest.FileTypes.Base.Models;

namespace PixChest.FileTypes.Video.Models;
public class VideoFileModel(long id, string filePath) : BaseFileModel(id, filePath, _fileOperator) {
	private static readonly VideoFileOperator _fileOperator = new();
}
