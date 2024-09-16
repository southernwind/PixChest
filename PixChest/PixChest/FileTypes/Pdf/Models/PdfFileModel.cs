using PixChest.FileTypes.Base.Models;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Pdf.Models;
public class PdfFileModel(long id, string filePath) : BaseFileModel(id, filePath, _fileOperator) {
	private static readonly PdfFileOperator _fileOperator = new();
	public override MediaType MediaType {
		get;
	} = MediaType.Pdf;
}
