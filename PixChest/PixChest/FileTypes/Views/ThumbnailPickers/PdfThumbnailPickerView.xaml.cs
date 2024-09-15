using PixChest.Composition.Bases;
using PixChest.FileTypes.ViewModels;

namespace PixChest.FileTypes.Views.ThumbnailPickers;
public sealed partial class PdfThumbnailPickerView : PdfThumbnailPickerViewUserControl {
	public PdfThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class PdfThumbnailPickerViewUserControl : UserControlBase<PdfThumbnailPickerViewModel> {
}
