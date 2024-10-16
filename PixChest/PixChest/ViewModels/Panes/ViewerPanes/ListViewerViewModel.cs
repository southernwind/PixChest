using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class ListViewerViewModel : ViewerPaneViewModelBase {
	public ListViewerViewModel(FilesManager filesManager) : base ("List", filesManager){
	}
}
