using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels;
public class MainWindowViewModel: ViewModelBase {

	public ViewerSelectorViewModel ViewerSelectorViewModel {
		get;
	}

	public MainWindowViewModel(ViewerSelectorViewModel viewerSelectorViewModel) {
		this.ViewerSelectorViewModel = viewerSelectorViewModel;
	}
}
