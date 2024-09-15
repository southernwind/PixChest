using PixChest.FileTypes.Models.Operators;

namespace PixChest.FileTypes.Models.Files;
public class ImageFileModel(long id, string filePath) : FileModel(id, filePath, _fileOperator) {
	private static readonly ImageFileOperator _fileOperator = new();
}