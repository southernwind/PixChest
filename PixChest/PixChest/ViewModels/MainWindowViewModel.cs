using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels;
public class MainWindowViewModel: ViewModelBase {

	public NavigationMenuViewModel ViewerSelectorViewModel {
		get;
	}

	public MainWindowViewModel(NavigationMenuViewModel viewerSelectorViewModel) {
		this.ViewerSelectorViewModel = viewerSelectorViewModel;
	}
}
