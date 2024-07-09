using Microsoft.UI.Xaml.Input;
using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.Views.Panes.ViewerPanes;

public class ViewerPaneBase: UserControlBase<ViewerSelectorViewModel> {

	protected void File_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
		this.ViewModel?.MediaContentLibraryViewModel.ExecuteCommand.Execute(Unit.Default);
	}
}
