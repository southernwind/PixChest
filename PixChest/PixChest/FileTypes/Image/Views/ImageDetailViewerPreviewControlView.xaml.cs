using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Views;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.FileTypes.Image.Views;
public sealed partial class ImageDetailViewerPreviewControlView : ImageDetailViewerPreviewControlViewUserControl, IDetailViewerPreviewControlView {
	public ImageDetailViewerPreviewControlView() {
		this.InitializeComponent();
	}
}

public class ImageDetailViewerPreviewControlViewUserControl : UserControlBase<DetailViewerViewModel> {
}

