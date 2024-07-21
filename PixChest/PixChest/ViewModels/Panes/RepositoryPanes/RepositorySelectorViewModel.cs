using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.RepositoryPanes;

[AddTransient]
public class RepositorySelectorViewModel: ViewModelBase {

	public RepositorySelectorViewModel(
		FolderRepositoryViewModel folderRepositoryViewModel) {
		this.FolderRepositoryViewModel = folderRepositoryViewModel;
		this.RepositoryPaneViewModels = [
			folderRepositoryViewModel
		];
		this.SelectedRepositoryPane.Value = folderRepositoryViewModel;
	}

	public BindableReactiveProperty<RepositoryViewModelBase> SelectedRepositoryPane {
		get;
	} = new();

	public RepositoryViewModelBase[] RepositoryPaneViewModels {
		get;
	}
	public FolderRepositoryViewModel FolderRepositoryViewModel {
		get;
	}
}
