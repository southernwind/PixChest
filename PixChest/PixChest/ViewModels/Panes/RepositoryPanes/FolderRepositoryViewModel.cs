using PixChest.Models.Repositories;
using PixChest.Models.Repositories.Objects;

namespace PixChest.ViewModels.Panes.RepositoryPanes;

[AddTransient]
public class FolderRepositoryViewModel : RepositoryViewModelBase {
	public FolderRepositoryViewModel(FolderRepository folderRepository) {
		this.RootFolder = folderRepository.RootFolder.ToBindableReactiveProperty(null!);
		this.LoadCommand.Subscribe(async _ => await folderRepository.Load());
	}
	public BindableReactiveProperty<FolderObject> RootFolder {
		get;
	}

	public ReactiveCommand<Unit> LoadCommand {
		get;
	} = new();
}
