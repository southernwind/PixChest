using PixChest.FileTypes.Models.Operators;
using PixChest.Models.FileDetailManagers;
using PixChest.ViewModels.Thumbnails.FileTypes.Base;

namespace PixChest.ViewModels.Thumbnails.FileTypes.Pdf;

[AddTransient]
public class PdfThumbnailPickerViewModel: BaseThumbnailPickerViewModel {
	public PdfThumbnailPickerViewModel(
		ThumbnailsManager thumbnailsManager,
		PdfFileOperator imageFileOperator) : base(thumbnailsManager) {
		this._pdfFileOperator = imageFileOperator;
	}
	private readonly PdfFileOperator _pdfFileOperator;

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
