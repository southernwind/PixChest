using CommunityToolkit.Mvvm.DependencyInjection;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base;
using PixChest.FileTypes.Pdf.Models;
using PixChest.FileTypes.Pdf.ViewModels;
using PixChest.FileTypes.Pdf.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Pdf;
[AddTransient(typeof(IFileType))]
public class PdfFileType: BaseFileType<PdfFileOperator, PdfFileModel, PdfFileViewModel, PdfDetailViewerPreviewControlView, PdfThumbnailPickerViewModel, PdfThumbnailPickerView> {
	private PdfDetailViewerPreviewControlView? _pdfDetailViewerPreviewControlView;
	public override MediaType MediaType {
		get;
	} = MediaType.Pdf;

	public override PdfFileOperator CreateFileOperator() {
		return new PdfFileOperator();
	}

	public override PdfFileModel CreateFileModelFromRecord(MediaFile mediaFile) {
		var ifm = new PdfFileModel(mediaFile.MediaFileId, mediaFile.FilePath);
		this.SetModelProperties(ifm, mediaFile);
		return ifm;
	}

	public override PdfFileViewModel CreateFileViewModel(PdfFileModel fileModel) {
		return new PdfFileViewModel(fileModel);
	}
	public override PdfDetailViewerPreviewControlView CreateDetailViewerPreviewControlView(PdfFileViewModel fileViewModel) {
		return this._pdfDetailViewerPreviewControlView ??= new PdfDetailViewerPreviewControlView();
	}

	public override PdfThumbnailPickerViewModel CreateThumbnailPickerViewModel() {
		return Ioc.Default.GetRequiredService<PdfThumbnailPickerViewModel>();
	}

	public override PdfThumbnailPickerView CreateThumbnailPickerView() {
		return new PdfThumbnailPickerView();
	}
	public override IQueryable<MediaFile> IncludeTables(IQueryable<MediaFile> mediaFiles) {
		return mediaFiles
			.Include(mf => mf.Container);
	}
}
