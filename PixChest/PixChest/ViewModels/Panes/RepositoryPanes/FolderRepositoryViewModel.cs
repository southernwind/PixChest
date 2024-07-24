using PixChest.Models.Repositories;
using PixChest.Models.Repositories.Objects;

namespace PixChest.ViewModels.Panes.RepositoryPanes;

[AddTransient]
public class FolderRepositoryViewModel : RepositoryViewModelBase {
	public FolderRepositoryViewModel(FolderRepository folderRepository): base(folderRepository) {
		this.RootFolder = folderRepository.RootFolder.ToBindableReactiveProperty(null!);
		this.SetRepositoryCondition.Subscribe(_ => {
			if(this.SelectedFolder.Value is not { } folder) {
				return;
			}
			folderRepository.SetRepositoryCandidate(folder);
		});
	}
	public BindableReactiveProperty<FolderObject> RootFolder {
		get;
	}

	public BindableReactiveProperty<FolderObject?> SelectedFolder {
		get;
	} = new();

	public ReactiveCommand<Unit> SetRepositoryCondition {
		get;
	} = new();
}
