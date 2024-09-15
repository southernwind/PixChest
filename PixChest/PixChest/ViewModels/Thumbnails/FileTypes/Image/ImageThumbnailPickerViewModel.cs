using System.IO;
using System.Threading.Tasks;

using PixChest.FileTypes.Models.Operators;
using PixChest.Models.FileDetailManagers;
using PixChest.ViewModels.Thumbnails.FileTypes.Base;

namespace PixChest.ViewModels.Thumbnails.FileTypes.Image;

[AddTransient]
public class ImageThumbnailPickerViewModel: BaseThumbnailPickerViewModel {
	public ImageThumbnailPickerViewModel(
		ThumbnailsManager thumbnailsManager,
		ImageFileOperator imageFileOperator):base(thumbnailsManager){
		this._imageFileOperator = imageFileOperator;
	}
	private readonly ImageFileOperator _imageFileOperator;

	public override void RecreateThumbnail() {
		if (this.targetFileViewModel is null) {
			return;
		}
		this.CandidateThumbnail.Value = this._imageFileOperator.CreateThumbnail(this.targetFileViewModel.FileModel, 300, 300);
	}
}
