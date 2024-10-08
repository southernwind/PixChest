using CommunityToolkit.Mvvm.DependencyInjection;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base;
using PixChest.FileTypes.Video.Models;
using PixChest.FileTypes.Video.ViewModels;
using PixChest.FileTypes.Video.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Video;
[AddTransient(typeof(IFileType))]
public class VideoFileType: BaseFileType<VideoFileOperator, VideoFileModel, VideoFileViewModel, VideoDetailViewerPreviewControlView, VideoThumbnailPickerViewModel, VideoThumbnailPickerView> {
	private VideoDetailViewerPreviewControlView? _videoDetailViewerPreviewControlView;
	public override MediaType MediaType {
		get;
	} = MediaType.Video;

	public override VideoFileOperator CreateFileOperator() {
		return new VideoFileOperator();
	}

	public override VideoFileModel CreateFileModelFromRecord(MediaFile mediaFile) {
		var ifm = new VideoFileModel(mediaFile.MediaFileId, mediaFile.FilePath);
		this.SetModelProperties(ifm, mediaFile);
		return ifm;
	}

	public override VideoFileViewModel CreateFileViewModel(VideoFileModel fileModel) {
		return new VideoFileViewModel(fileModel);
	}

	public override VideoDetailViewerPreviewControlView CreateDetailViewerPreviewControlView(VideoFileViewModel fileViewModel) {
		return this._videoDetailViewerPreviewControlView ??= new VideoDetailViewerPreviewControlView();
	}

	public override VideoThumbnailPickerViewModel CreateThumbnailPickerViewModel() {
		return Ioc.Default.GetRequiredService<VideoThumbnailPickerViewModel>();
	}

	public override VideoThumbnailPickerView CreateThumbnailPickerView() {
		return new VideoThumbnailPickerView();
	}
	public override IQueryable<MediaFile> IncludeTables(IQueryable<MediaFile> mediaFiles) {
		return mediaFiles
			.Include(mf => mf.VideoFile);
	}
}
