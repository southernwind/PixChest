using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.Views.Panes.ViewerPanes;
public sealed partial class ViewerSelector : ViewerSelectorUserControl {
	public ViewerSelector() {
		this.InitializeComponent();
	}
}
public abstract class ViewerSelectorUserControl: UserControlBase<ViewerSelectorViewModel>;
