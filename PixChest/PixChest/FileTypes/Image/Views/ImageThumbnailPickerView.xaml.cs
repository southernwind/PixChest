using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Views;
using PixChest.FileTypes.Image.ViewModels;

namespace PixChest.FileTypes.Image.Views;
public sealed partial class ImageThumbnailPickerView : ImageThumbnailPickerViewUserControl, IThumbnailPickerView {
	public ImageThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class ImageThumbnailPickerViewUserControl : UserControlBase<ImageThumbnailPickerViewModel> {
}
