using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Pdf.ViewModels;
public class PdfFileViewModel(IFileModel fileModel) : BaseFileViewModel(fileModel) {
	public override MediaType MediaType {
		get;
	} = MediaType.Pdf;
}
