using PixChest.Models.Files.FileTypes.Base;

namespace PixChest.Models.Files.FileTypes.Pdf;
public class PdfFileModel(long id, string filePath) : FileModel(id, filePath, _fileOperator) {
	private static readonly PdfFileOperator _fileOperator = new();
}
