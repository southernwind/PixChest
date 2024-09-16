using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.FileTypes.Base.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Base;
public interface IFileType<TFileOperator, TFileModel, TFileViewModel, TThumbnailPickerViewModel, TThumbnailPickerView>:IFileType
	where TFileOperator: IFileOperator
	where TFileModel: IFileModel
	where TFileViewModel : IFileViewModel
	where TThumbnailPickerViewModel: IThumbnailPickerViewModel
	where TThumbnailPickerView: IThumbnailPickerView {
	public new TFileOperator CreateFileOperator();
	public new TFileModel CreateFileModelFromRecord(MediaFile mediaFile);
	public TFileViewModel CreateFileViewModel(TFileModel fileModel);
	public new TThumbnailPickerViewModel CreateThumbnailPickerViewModel();
	public new TThumbnailPickerView CreateThumbnailPickerView();
}
public interface IFileType {
	public MediaType MediaType {
		get;
	}
	public IFileOperator CreateFileOperator();
	public IFileModel CreateFileModelFromRecord(MediaFile mediaFile);
	public IFileViewModel CreateFileViewModel(IFileModel fileModel);
	public IThumbnailPickerViewModel CreateThumbnailPickerViewModel();
	public IThumbnailPickerView CreateThumbnailPickerView();
	public IQueryable<MediaFile> IncludeTables(IQueryable<MediaFile> mediaFiles);
}
