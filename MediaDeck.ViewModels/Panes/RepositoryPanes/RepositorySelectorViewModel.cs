using MediaDeck.Common.Base;
using MediaDeck.Common.Extensions;
using MediaDeck.Composition.Stores.State.Model;
using MediaDeck.Core.Models.Repositories;

namespace MediaDeck.ViewModels.Panes.RepositoryPanes;

[Inject(InjectServiceLifetime.Scoped)]
public class RepositorySelectorViewModel : ViewModelBase {
	public RepositorySelectorViewModel(
		TabStateModel tabState,
		RepositorySelector repositorySelector,
		FolderRepositoryViewModel folderRepositoryViewModel,
		AlbumRepositoryViewModel albumRepositoryViewModel) {
		this.RepositoryPaneViewModels = [folderRepositoryViewModel, albumRepositoryViewModel];
		this.FolderRepositoryViewModel = folderRepositoryViewModel;
		this.AlbumRepositoryViewModel = albumRepositoryViewModel;

		// Stateと双方向バインドするプロパティの作成
		this.SelectedRepositoryPane = tabState.ActiveRepository.ToTwoWayBindableReactiveProperty<RepositoryType, RepositoryViewModelBase>(
			type => type switch {
				RepositoryType.Folder => folderRepositoryViewModel,
				RepositoryType.Album => albumRepositoryViewModel,
				_ => folderRepositoryViewModel
			},
			vm => vm switch {
				FolderRepositoryViewModel _ => RepositoryType.Folder,
				AlbumRepositoryViewModel _ => RepositoryType.Album,
				_ => RepositoryType.Folder
			},
			folderRepositoryViewModel,
			this.CompositeDisposable
		);

		// Model への反映
		this.SelectedRepositoryPane.Subscribe(vm => {
			if (vm is { } v) {
				repositorySelector.SelectedRepository.Value = v.Model;
			}
		}).AddTo(this.CompositeDisposable);
		this.LoadCommand.SubscribeAwait(async (_, ct) => {
			foreach (var repository in repositorySelector.Repositories) {
				await repository.Load();
			}
		}, AwaitOperation.Drop).AddTo(this.CompositeDisposable);
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