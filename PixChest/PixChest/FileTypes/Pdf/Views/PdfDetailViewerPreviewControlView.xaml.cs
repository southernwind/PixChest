using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Views;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.FileTypes.Pdf.Views;
public sealed partial class PdfDetailViewerPreviewControlView : PdfDetailViewerPreviewControlViewUserControl, IDetailViewerPreviewControlView {
	public PdfDetailViewerPreviewControlView() {
		this.InitializeComponent();
	}
}

public class PdfDetailViewerPreviewControlViewUserControl : UserControlBase<DetailViewerViewModel> {
}

