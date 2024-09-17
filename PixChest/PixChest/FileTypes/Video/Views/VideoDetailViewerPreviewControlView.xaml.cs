using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Views;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.FileTypes.Video.Views;
public sealed partial class VideoDetailViewerPreviewControlView : VideoDetailViewerPreviewControlViewUserControl, IDetailViewerPreviewControlView {
	public VideoDetailViewerPreviewControlView() {
		this.InitializeComponent();
	}
}

public class VideoDetailViewerPreviewControlViewUserControl : UserControlBase<DetailViewerViewModel> {
}

