using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;

public abstract class ViewerPaneViewModelBase(string name) : ViewModelBase {
	public string Name {
		get;
	} = name;
}
