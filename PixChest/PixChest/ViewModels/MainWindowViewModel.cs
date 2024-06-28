using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels;

[AddSingleton]
public class MainWindowViewModel(ViewerSelectorViewModel viewerSelectorViewModel, NavigationMenuViewModel navigationMenuViewModel) : ViewModelBase {

	public ViewerSelectorViewModel ViewerSelectorViewModel {
		get;
	} = viewerSelectorViewModel;

	public NavigationMenuViewModel NavigationMenuViewModel {
		get;
	} = navigationMenuViewModel;
}
