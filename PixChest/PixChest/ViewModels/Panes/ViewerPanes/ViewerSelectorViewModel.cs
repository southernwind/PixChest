using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class ViewerSelectorViewModel(DetailViewerViewModel detailViewerViewModel) : ViewModelBase {
	public DetailViewerViewModel DetailViewerViewModel {
		get;
	} = detailViewerViewModel;
}
