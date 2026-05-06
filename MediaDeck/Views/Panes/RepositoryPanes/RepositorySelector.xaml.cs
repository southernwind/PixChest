using MediaDeck.ViewModels.Panes.RepositoryPanes;

namespace MediaDeck.Views.Panes.RepositoryPanes;

public sealed partial class RepositorySelector {
	public RepositorySelector() {
		this.InitializeComponent();
		this.Loaded += (_, _) => {
			this.Bindings.Update();
		};
	}

	protected override void OnViewModelChanged(RepositorySelectorViewModel? oldViewModel, RepositorySelectorViewModel? newViewModel) {
		base.OnViewModelChanged(oldViewModel, newViewModel);
		newViewModel?.LoadCommand.Execute(Unit.Default);
	}
}

public abstract class RepositorySelectorUserControl : UserControlBase<RepositorySelectorViewModel>;
