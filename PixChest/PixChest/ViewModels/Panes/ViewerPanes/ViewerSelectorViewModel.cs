using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class ViewerSelectorViewModel : ViewModelBase {
	public ViewerSelectorViewModel(WrapViewerViewModel wrapViewerViewModel,ListViewerViewModel listViewerViewModel) {
		this.ViewerPaneViewModels = [
			wrapViewerViewModel,
			listViewerViewModel
		];
		this.SelectedViewerPane.Value = wrapViewerViewModel;
		this.WrapViewerViewModel = wrapViewerViewModel;
		this.ListViewerViewModel = listViewerViewModel;
	}

	public BindableReactiveProperty<ViewerPaneViewModelBase> SelectedViewerPane {
		get;
	} = new();

	public ViewerPaneViewModelBase[] ViewerPaneViewModels {
		get;
	}
	public ViewerPaneViewModelBase WrapViewerViewModel {
		get;
	}
	public ViewerPaneViewModelBase ListViewerViewModel {
		get;
	}
}
