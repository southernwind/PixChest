using PixChest.FileTypes.Base.ViewModels;
using PixChest.FileTypes.Pdf.Models;
using PixChest.Models.FileDetailManagers;

namespace PixChest.FileTypes.Pdf.ViewModels;

[AddTransient]
public class PdfThumbnailPickerViewModel(ThumbnailsManager thumbnailsManager, PdfFileOperator pdfFileOperator) : BaseThumbnailPickerViewModel(thumbnailsManager) {
	private readonly PdfFileOperator _pdfFileOperator = pdfFileOperator;

	public BindableReactiveProperty<int> PageNumber {
		get;
	} = new(1);

	public override void RecreateThumbnail() {
		if (this.targetFileViewModel is null) {
			return;
		}
		try {
			this.CandidateThumbnail.Value = this._pdfFileOperator.CreateThumbnail(this.targetFileViewModel.FilePath, 300, 300, this.PageNumber.Value);
		} catch (Exception) {
			this.CandidateThumbnail.Value = null;
		}
	}
}
