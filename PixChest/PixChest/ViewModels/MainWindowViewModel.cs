using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.DetailPanes;
using PixChest.ViewModels.Panes.FilterPanes;
using PixChest.ViewModels.Panes.RepositoryPanes;
using PixChest.ViewModels.Panes.ViewerPanes;
using PixChest.ViewModels.Tools;

namespace PixChest.ViewModels;

[AddSingleton]
public class MainWindowViewModel : ViewModelBase {
	public MainWindowViewModel(
		ViewerSelectorViewModel viewerSelectorViewModel,
		NavigationMenuViewModel navigationMenuViewModel,
		FilterSelectorViewModel filterSelectorViewModel,
		DetailSelectorViewModel detailSelectorViewModel,
		RepositorySelectorViewModel repositorySelectorViewModel,
		BackgroundTasksViewModel backgroundTasksViewModel) {
		this.ViewerSelectorViewModel = viewerSelectorViewModel;
		this.NavigationMenuViewModel = navigationMenuViewModel;
		this.FilterSelectorViewModel = filterSelectorViewModel;
		this.DetailSelectorViewModel = detailSelectorViewModel;
		this.RepositorySelectorViewModel = repositorySelectorViewModel;
		this.ViewerSelectorViewModel.MediaContentLibraryViewModel.SelectedFiles.Subscribe(x => {
			this.DetailSelectorViewModel.TargetFiles.Value = x;
		});
		this.WindowActivatedCommand.Subscribe(_ => {
			backgroundTasksViewModel.Start();
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
	public RepositorySelectorViewModel RepositorySelectorViewModel {
		get;
	}

	public ReactiveCommand WindowActivatedCommand {
		get;
	} = new();
}
