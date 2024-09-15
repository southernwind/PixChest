using PixChest.Composition.Bases;
using PixChest.FileTypes.Image.ViewModels;

namespace PixChest.FileTypes.Image.Views;
public sealed partial class ImageThumbnailPickerView : ImageThumbnailPickerViewUserControl {
	public ImageThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class ImageThumbnailPickerViewUserControl : UserControlBase<ImageThumbnailPickerViewModel> {
}
