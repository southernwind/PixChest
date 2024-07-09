using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;

public abstract class ViewerPaneViewModelBase : ViewModelBase {
	public ViewerPaneViewModelBase(string name) {
		this.Name = name;
	}

	public string Name {
		get;
	}
}
