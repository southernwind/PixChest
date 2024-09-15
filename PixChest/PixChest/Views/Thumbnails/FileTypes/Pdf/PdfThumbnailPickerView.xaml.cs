using PixChest.Composition.Bases;
using PixChest.FileTypes.ViewModels;

namespace PixChest.Views.Thumbnails.FileTypes.Pdf;
public sealed partial class PdfThumbnailPickerView : PdfThumbnailPickerViewUserControl {
	public PdfThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class PdfThumbnailPickerViewUserControl : UserControlBase<PdfThumbnailPickerViewModel> {
}
