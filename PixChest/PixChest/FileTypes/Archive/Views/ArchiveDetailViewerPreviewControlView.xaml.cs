using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Views;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.FileTypes.Archive.Views;
public sealed partial class ArchiveDetailViewerPreviewControlView : ArchiveDetailViewerPreviewControlViewUserControl, IDetailViewerPreviewControlView {
	public ArchiveDetailViewerPreviewControlView() {
		this.InitializeComponent();
	}
}

public class ArchiveDetailViewerPreviewControlViewUserControl : UserControlBase<DetailViewerViewModel> {
}

