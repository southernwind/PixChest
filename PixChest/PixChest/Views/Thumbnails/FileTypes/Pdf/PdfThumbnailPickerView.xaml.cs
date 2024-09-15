using PixChest.Composition.Bases;
using PixChest.ViewModels.Thumbnails.FileTypes.Pdf;

namespace PixChest.Views.Thumbnails.FileTypes.Pdf;
public sealed partial class PdfThumbnailPickerView : PdfThumbnailPickerViewUserControl {
	public PdfThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class PdfThumbnailPickerViewUserControl : UserControlBase<PdfThumbnailPickerViewModel> {
}
