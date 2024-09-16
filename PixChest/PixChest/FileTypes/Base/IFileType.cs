using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.FileTypes.Base.Views;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Base;
public interface IFileType<TFileOperator, TFileModel, TThumbnailPickerViewModel, TThumbnailPickerView>:IFileType
	where TFileOperator: IFileOperator
	where TFileModel: IFileModel
	where TThumbnailPickerViewModel: IThumbnailPickerViewModel
	where TThumbnailPickerView: IThumbnailPickerView {

	public new TFileModel CreateFileModelFromRecord(MediaFile mediaFile);
	public new TFileOperator CreateFileOperator();
	public new TThumbnailPickerViewModel CreateThumbnailPickerViewModel();
	public new TThumbnailPickerView CreateThumbnailPickerView();
}
public interface IFileType {
	public MediaType MediaType {
		get;
	}

	public IFileModel CreateFileModelFromRecord(MediaFile mediaFile);
	public IFileOperator CreateFileOperator();
	public IThumbnailPickerViewModel CreateThumbnailPickerViewModel();
	public IThumbnailPickerView CreateThumbnailPickerView();
}
