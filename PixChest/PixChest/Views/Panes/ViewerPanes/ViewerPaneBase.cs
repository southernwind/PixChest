using Microsoft.UI.Xaml.Input;
using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.Views.Panes.ViewerPanes;

public class ViewerPaneBase<T>: UserControlBase<T> where T : ViewerPaneViewModelBase {

	protected void File_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
		this.ViewModel?.ExecuteCommand.Execute(Unit.Default);
	}
}
