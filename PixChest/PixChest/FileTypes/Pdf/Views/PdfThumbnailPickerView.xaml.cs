using PixChest.Composition.Bases;
using PixChest.FileTypes.Pdf.ViewModels;

namespace PixChest.FileTypes.Pdf.Views;
public sealed partial class PdfThumbnailPickerView : PdfThumbnailPickerViewUserControl {
	public PdfThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class PdfThumbnailPickerViewUserControl : UserControlBase<PdfThumbnailPickerViewModel> {
}
