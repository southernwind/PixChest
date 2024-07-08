using PixChest.ViewModels.Panes.ViewerPanes;
namespace PixChest.Views.Panes.ViewerPanes;
public sealed partial class DetailViewer : DetailViewerUserControl {
	public DetailViewer() {
		this.InitializeComponent();
	}
}
public abstract class DetailViewerUserControl : ViewerPaneBase<DetailViewerViewModel>;

