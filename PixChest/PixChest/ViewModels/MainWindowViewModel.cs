using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.FilterPanes;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels;

[AddSingleton]
public class MainWindowViewModel(ViewerSelectorViewModel viewerSelectorViewModel, NavigationMenuViewModel navigationMenuViewModel, FilterSelectorViewModel filterSelectorViewModel) : ViewModelBase {

	public ViewerSelectorViewModel ViewerSelectorViewModel {
		get;
	} = viewerSelectorViewModel;

	public NavigationMenuViewModel NavigationMenuViewModel {
		get;
	} = navigationMenuViewModel;

	public FilterSelectorViewModel FilterSelectorViewModel {
		get;
	} = filterSelectorViewModel;
}
