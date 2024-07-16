using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PixChest.Composition.Bases;
using PixChest.ViewModels.Files;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.Views.Panes.ViewerPanes;

public class ViewerPaneBase: UserControlBase<ViewerSelectorViewModel> {
	protected virtual void List_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.ViewModel is not { } vm || sender is not ListBox listBox) {
			return;
		}
		vm.MediaContentLibraryViewModel.SelectedFiles.Value = listBox.SelectedItems.Select(x => x as FileViewModel).Where(x => x is not null).ToArray()!;
	}

	protected void File_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
		if (sender is not Grid grid) {
			return;
		}
		if(grid.DataContext is not FileViewModel fileViewModel){
			return;
		}
		fileViewModel.ExecuteFile();
	}
}
