using PixChest.Models.Files.FileTypes.Base;

namespace PixChest.Models.Files.FileTypes.Image;
public class ImageFileModel(long id, string filePath) : FileModel(id, filePath, _fileOperator) {
	private static readonly ImageFileOperator _fileOperator = new();
}
