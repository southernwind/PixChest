using MediaDeck.Common.Base;
using MediaDeck.Core.Models.Repositories;

namespace MediaDeck.ViewModels.Panes.RepositoryPanes;

[Inject(InjectServiceLifetime.Scoped)]
public class RepositorySelectorViewModel : ViewModelBase {
	public RepositorySelectorViewModel(
		RepositorySelector repositorySelector,
		FolderRepositoryViewModel folderRepositoryViewModel,
		AlbumRepositoryViewModel albumRepositoryViewModel) {
		this.RepositoryPaneViewModels = [folderRepositoryViewModel, albumRepositoryViewModel];
		this.FolderRepositoryViewModel = folderRepositoryViewModel;
		this.AlbumRepositoryViewModel = albumRepositoryViewModel;
		this.SelectedRepositoryPane = repositorySelector.SelectedRepository.Select(x => this.RepositoryPaneViewModels.First(vm => vm.Model == x)).ToBindableReactiveProperty(null!);
		this.SelectedRepositoryPane.Subscribe(vm => {
			if (vm is { } v) {
				repositorySelector.SelectedRepository.Value = v.Model;
			}
		}).AddTo(this.CompositeDisposable);
		this.LoadCommand.Subscribe(async _ => {
			foreach (var repository in repositorySelector.Repositories) {
				await repository.Load();
			}
		}).AddTo(this.CompositeDisposable);
	}

	public ReactiveCommand LoadCommand {
		get;
	} = new();

	public BindableReactiveProperty<RepositoryViewModelBase> SelectedRepositoryPane {
		get;
	} = new();

	public RepositoryViewModelBase[] RepositoryPaneViewModels {
		get;
	}

	public FolderRepositoryViewModel FolderRepositoryViewModel {
		get;
	}

	public AlbumRepositoryViewModel AlbumRepositoryViewModel {
		get;
	}
}