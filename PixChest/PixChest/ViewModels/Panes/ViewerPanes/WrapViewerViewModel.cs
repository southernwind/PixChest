using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class WrapViewerViewModel : ViewerPaneViewModelBase {
	public WrapViewerViewModel(FilesManager filesManager) : base ("Wrap", filesManager){
	}
}
