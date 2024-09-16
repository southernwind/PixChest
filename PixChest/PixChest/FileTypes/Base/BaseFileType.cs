using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.FileTypes.Base.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Base;
public abstract class BaseFileType<TFileOperator, TFileModel, TFileViewModel, TThumbnailPickerViewModel, TThumbnailPickerView> : IFileType<TFileOperator, TFileModel, TFileViewModel, TThumbnailPickerViewModel, TThumbnailPickerView>
	where TFileOperator : IFileOperator
	where TFileModel : IFileModel
	where TFileViewModel : IFileViewModel
	where TThumbnailPickerViewModel : IThumbnailPickerViewModel
	where TThumbnailPickerView : IThumbnailPickerView {
	public abstract MediaType MediaType {
		get;
	}

	public abstract TFileOperator CreateFileOperator();
	public abstract TFileModel CreateFileModelFromRecord(MediaFile mediaFile);
	public abstract TFileViewModel CreateFileViewModel(TFileModel fileModel);
	public abstract TThumbnailPickerViewModel CreateThumbnailPickerViewModel();
	public abstract TThumbnailPickerView CreateThumbnailPickerView();
	public abstract IQueryable<MediaFile> IncludeTables(IQueryable<MediaFile> mediaFiles);

	protected void SetModelProperties(TFileModel fileModel, MediaFile mediaFile) {
		fileModel.ThumbnailFilePath = mediaFile.ThumbnailFileName;
		fileModel.Rate = mediaFile.Rate;
		fileModel.Description = mediaFile.Description;
		fileModel.UsageCount = mediaFile.UsageCount;
		fileModel.FileSize = mediaFile.FileSize;
		fileModel.CreationTime = mediaFile.CreationTime;
		fileModel.ModifiedTime = mediaFile.ModifiedTime;
		fileModel.LastAccessTime = mediaFile.LastAccessTime;
		fileModel.Tags = mediaFile.MediaFileTags.Select(mft => mft.Tag.TagName).ToList();
	}

	IFileOperator IFileType.CreateFileOperator() {
		return this.CreateFileOperator();
	}

	IFileModel IFileType.CreateFileModelFromRecord(MediaFile mediaFile) {
		return this.CreateFileModelFromRecord(mediaFile);
	}

	IFileViewModel IFileType.CreateFileViewModel(IFileModel fileModel) {
		return this.CreateFileViewModel((TFileModel)fileModel);
	}

	IThumbnailPickerViewModel IFileType.CreateThumbnailPickerViewModel() {
		return this.CreateThumbnailPickerViewModel();
	}

	IThumbnailPickerView IFileType.CreateThumbnailPickerView() {
		return this.CreateThumbnailPickerView();
	}
}
