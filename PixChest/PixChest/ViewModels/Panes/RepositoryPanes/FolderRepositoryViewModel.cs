using PixChest.Models.Files;
using PixChest.Models.Repositories;
using PixChest.Models.Repositories.Objects;

namespace PixChest.ViewModels.Panes.RepositoryPanes;

[AddTransient]
public class FolderRepositoryViewModel : RepositoryViewModelBase {
	public FolderRepositoryViewModel(FolderRepository folderRepository, MediaContentLibrary mediaContentLibrary) : base(folderRepository) {
		this.RootFolder = folderRepository.RootFolder.ToBindableReactiveProperty(null!);
		this.SetRepositoryConditionCommand.Subscribe(async _ => {
			if(this.SelectedFolder.Value is not { } folder) {
				return;
			}
			folderRepository.SetRepositoryCandidate(folder);
			await mediaContentLibrary.SearchAsync();
		});
	}
	public BindableReactiveProperty<FolderObject> RootFolder {
		get;
	}

	public BindableReactiveProperty<FolderObject?> SelectedFolder {
		get;
	} = new();

	public ReactiveCommand<Unit> SetRepositoryConditionCommand {
		get;
	} = new();
}
