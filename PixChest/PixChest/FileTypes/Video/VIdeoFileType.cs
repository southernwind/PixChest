using CommunityToolkit.Mvvm.DependencyInjection;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base;
using PixChest.FileTypes.Video.Models;
using PixChest.FileTypes.Video.ViewModels;
using PixChest.FileTypes.Video.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Video;
[AddTransient(typeof(IFileType))]
public class VideoFileType: BaseFileType<VideoFileOperator, VideoFileModel, VideoThumbnailPickerViewModel, VideoThumbnailPickerView> {
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
