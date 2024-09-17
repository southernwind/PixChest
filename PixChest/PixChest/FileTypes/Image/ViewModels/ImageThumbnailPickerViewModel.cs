using PixChest.FileTypes.Base.ViewModels;
using PixChest.FileTypes.Image.Models;
using PixChest.Models.FileDetailManagers;

namespace PixChest.FileTypes.Image.ViewModels;

[AddTransient]
public class ImageThumbnailPickerViewModel(ThumbnailsManager thumbnailsManager, ImageFileOperator imageFileOperator) : BaseThumbnailPickerViewModel(thumbnailsManager) {
	private readonly ImageFileOperator _imageFileOperator = imageFileOperator;

	public override void RecreateThumbnail() {
		if (this.targetFileViewModel is null) {
			return;
		}
		this.CandidateThumbnail.Value = this._imageFileOperator.CreateThumbnail(this.targetFileViewModel.FileModel, 300, 300);
	}
}
