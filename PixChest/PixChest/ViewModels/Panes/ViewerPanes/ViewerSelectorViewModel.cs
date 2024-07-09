using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class ViewerSelectorViewModel : ViewModelBase {
	public ViewerSelectorViewModel(MediaContentLibraryViewModel mediaContentLibraryViewModel, WrapViewerViewModel wrapViewerViewModel,ListViewerViewModel listViewerViewModel) {
		this.MediaContentLibraryViewModel = mediaContentLibraryViewModel;
		this.ViewerPaneViewModels = [
			wrapViewerViewModel,
			listViewerViewModel
		];
		this.SelectedViewerPane.Value = wrapViewerViewModel;
		this.WrapViewerViewModel = wrapViewerViewModel;
		this.ListViewerViewModel = listViewerViewModel;
	}

	public MediaContentLibraryViewModel MediaContentLibraryViewModel {
		get;
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
