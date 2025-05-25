using CommunityToolkit.Mvvm.DependencyInjection;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base;
using PixChest.FileTypes.Archive.Models;
using PixChest.FileTypes.Archive.ViewModels;
using PixChest.FileTypes.Archive.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Archive;
[AddTransient(typeof(IFileType))]
public class ArchiveFileType: BaseFileType<ArchiveFileOperator, ArchiveFileModel, ArchiveFileViewModel, ArchiveDetailViewerPreviewControlView, ArchiveThumbnailPickerViewModel, ArchiveThumbnailPickerView> {
	private ArchiveDetailViewerPreviewControlView? _archiveDetailViewerPreviewControlView;
	public override MediaType MediaType {
		get;
	} = MediaType.Archive;

	public override ArchiveFileOperator CreateFileOperator() {
		return new ArchiveFileOperator();
	}

	public override ArchiveFileModel CreateFileModelFromRecord(MediaFile mediaFile) {
		var ifm = new ArchiveFileModel(mediaFile.MediaFileId, mediaFile.FilePath);
		this.SetModelProperties(ifm, mediaFile);
		return ifm;
	}

	public override ArchiveFileViewModel CreateFileViewModel(ArchiveFileModel fileModel) {
		return new ArchiveFileViewModel(fileModel);
	}
	public override ArchiveDetailViewerPreviewControlView CreateDetailViewerPreviewControlView(ArchiveFileViewModel fileViewModel) {
		return this._archiveDetailViewerPreviewControlView ??= new ArchiveDetailViewerPreviewControlView();
	}

	public override ArchiveThumbnailPickerViewModel CreateThumbnailPickerViewModel() {
		return Ioc.Default.GetRequiredService<ArchiveThumbnailPickerViewModel>();
	}

	public override ArchiveThumbnailPickerView CreateThumbnailPickerView() {
		return new ArchiveThumbnailPickerView();
	}
	public override IQueryable<MediaFile> IncludeTables(IQueryable<MediaFile> mediaFiles) {
		return mediaFiles
			.Include(mf => mf.Container);
	}
}
