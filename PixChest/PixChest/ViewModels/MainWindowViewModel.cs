using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.DetailPanes;
using PixChest.ViewModels.Panes.FilterPanes;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels;

[AddSingleton]
public class MainWindowViewModel : ViewModelBase {
	public MainWindowViewModel(
		ViewerSelectorViewModel viewerSelectorViewModel,
		NavigationMenuViewModel navigationMenuViewModel,
		FilterSelectorViewModel filterSelectorViewModel,
		DetailSelectorViewModel detailSelectorViewModel) {
		this.ViewerSelectorViewModel = viewerSelectorViewModel;
		this.NavigationMenuViewModel = navigationMenuViewModel;
		this.FilterSelectorViewModel = filterSelectorViewModel;
		this.DetailSelectorViewModel = detailSelectorViewModel;

		this.ViewerSelectorViewModel.MediaContentLibraryViewModel.SelectedFile.Subscribe(x => {
			this.DetailSelectorViewModel.TargetFile.Value = x;
		});
	}

	public ViewerSelectorViewModel ViewerSelectorViewModel {
		get;
	}

	public NavigationMenuViewModel NavigationMenuViewModel {
		get;
	}

	public FilterSelectorViewModel FilterSelectorViewModel {
		get;
	}
	public DetailSelectorViewModel DetailSelectorViewModel {
		get;
	}
}
