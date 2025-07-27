using PixChest.Models.Repositories;
using PixChest.Models.Repositories.Objects;

namespace PixChest.ViewModels.Panes.RepositoryPanes;

[AddTransient]
public class FolderRepositoryViewModel : RepositoryViewModelBase {
	public FolderRepositoryViewModel(FolderRepository folderRepository) : base(folderRepository) {
		this.RootFolder = folderRepository.RootFolder.ObserveOnCurrentSynchronizationContext().ToBindableReactiveProperty(null!);
		this.SetRepositoryConditionCommand.Merge(this.IncludeSubDirectories.ToUnit()).Subscribe(_ => {
			if(this.SelectedFolder.Value is not { } folder) {
				return;
			}
			folderRepository.SetRepositoryCandidate(folder, this.IncludeSubDirectories.Value);
		});
	}
	public BindableReactiveProperty<FolderObject> RootFolder {
		get;
	}

	public BindableReactiveProperty<FolderObject?> SelectedFolder {
		get;
	} = new();

	public ReactiveCommand SetRepositoryConditionCommand {
		get;
	} = new();

	public BindableReactiveProperty<bool> IncludeSubDirectories {
		get;
	} = new(true);
}
