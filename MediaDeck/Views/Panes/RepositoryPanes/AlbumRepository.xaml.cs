using MediaDeck.ViewModels.Panes.RepositoryPanes;
using Microsoft.UI.Xaml.Input;

namespace MediaDeck.Views.Panes.RepositoryPanes;

public sealed partial class AlbumRepository {
	public AlbumRepository() {
		this.InitializeComponent();
		this.Loaded += (_, _) => {
			this.Bindings.Update();
		};
	}

	private void TreeViewItem_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
		if (this.ViewModel is not { } vm) {
			return;
		}
		vm.AlbumRepositoryViewModel.SetRepositoryConditionCommand.Execute(Unit.Default);
	}
}

public abstract class AlbumRepositoryUserControl : UserControlBase<RepositorySelectorViewModel>;