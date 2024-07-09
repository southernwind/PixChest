using PixChest.ViewModels.Panes.ViewerPanes;
namespace PixChest.Views.Panes.ViewerPanes;
public sealed partial class WrapViewer : WrapViewerUserControl {
	public WrapViewer() {
		this.InitializeComponent();
	}
}
public abstract class WrapViewerUserControl : ViewerPaneBase<WrapViewerViewModel>;

