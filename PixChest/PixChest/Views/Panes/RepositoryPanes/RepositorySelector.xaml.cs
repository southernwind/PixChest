using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.RepositoryPanes;

namespace PixChest.Views.Panes.RepositoryPanes;
public sealed partial class RepositorySelector : RepositorySelectorUserControl {
	public RepositorySelector() {
		this.InitializeComponent();
	}
}

public abstract class RepositorySelectorUserControl : UserControlBase<RepositorySelectorViewModel>;