using PixChest.Composition.Bases;
using PixChest.FileTypes.ViewModels;

namespace PixChest.Views.Thumbnails.FileTypes.Image;
public sealed partial class ImageThumbnailPickerView : ImageThumbnailPickerViewUserControl {
	public ImageThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class ImageThumbnailPickerViewUserControl : UserControlBase<ImageThumbnailPickerViewModel> {
}
