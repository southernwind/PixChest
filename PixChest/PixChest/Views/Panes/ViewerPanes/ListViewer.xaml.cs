using PixChest.ViewModels.Panes.ViewerPanes;
namespace PixChest.Views.Panes.ViewerPanes;
public sealed partial class ListViewer : ListViewerUserControl {
	public ListViewer() {
		this.InitializeComponent();
	}
}
public abstract class ListViewerUserControl : ViewerPaneBase<ListViewerViewModel>;

