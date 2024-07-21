using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.RepositoryPanes;

namespace PixChest.Views.Panes.RepositoryPanes;
public sealed partial class FolderRepository : FolderRepositoryUserControl {
	public FolderRepository() {
		this.InitializeComponent();
	}

	protected override void OnViewModelChanged(RepositorySelectorViewModel? oldViewModel, RepositorySelectorViewModel? newViewModel) {
		base.OnViewModelChanged(oldViewModel, newViewModel);
		if (newViewModel == null) {
			return;
		}
		newViewModel.FolderRepositoryViewModel.LoadCommand.Execute(Unit.Default);
	}
}

public abstract class FolderRepositoryUserControl : UserControlBase<RepositorySelectorViewModel>;