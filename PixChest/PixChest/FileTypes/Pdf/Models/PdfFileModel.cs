using PixChest.FileTypes.Base.Models;

namespace PixChest.FileTypes.Pdf.Models;
public class PdfFileModel(long id, string filePath) : BaseFileModel(id, filePath, _fileOperator) {
	private static readonly PdfFileOperator _fileOperator = new();
}
