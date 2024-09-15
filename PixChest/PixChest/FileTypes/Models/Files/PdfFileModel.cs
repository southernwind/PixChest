using PixChest.FileTypes.Models.Operators;

namespace PixChest.FileTypes.Models.Files;
public class PdfFileModel(long id, string filePath) : FileModel(id, filePath, _fileOperator) {
	private static readonly PdfFileOperator _fileOperator = new();
}
