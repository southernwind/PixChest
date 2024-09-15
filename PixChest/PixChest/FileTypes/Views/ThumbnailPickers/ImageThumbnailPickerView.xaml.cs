using PixChest.Composition.Bases;
using PixChest.FileTypes.ViewModels;

namespace PixChest.FileTypes.Views.ThumbnailPickers;
public sealed partial class ImageThumbnailPickerView : ImageThumbnailPickerViewUserControl {
	public ImageThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class ImageThumbnailPickerViewUserControl : UserControlBase<ImageThumbnailPickerViewModel> {
}
