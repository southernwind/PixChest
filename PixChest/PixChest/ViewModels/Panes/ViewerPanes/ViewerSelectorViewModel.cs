using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;
public class ViewerSelectorViewModel : ViewModelBase {
	public DetailViewerViewModel DetailViewerViewModel {
		get;
	}

	public ViewerSelectorViewModel(DetailViewerViewModel detailViewerViewModel) {
		this.DetailViewerViewModel = detailViewerViewModel;
	}

}
