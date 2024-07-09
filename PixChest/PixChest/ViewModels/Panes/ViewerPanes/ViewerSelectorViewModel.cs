using PixChest.Composition.Bases;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class ViewerSelectorViewModel : ViewModelBase {
	public ViewerSelectorViewModel(
		MediaContentLibraryViewModel mediaContentLibraryViewModel,
		WrapViewerViewModel wrapViewerViewModel,
		ListViewerViewModel listViewerViewModel,
		DetailViewerViewModel detailViewerViewModel) {
		this.MediaContentLibraryViewModel = mediaContentLibraryViewModel;
		this.ViewerPaneViewModels = [
			wrapViewerViewModel,
			listViewerViewModel,
			detailViewerViewModel
		];
		this.SelectedViewerPane.Value = wrapViewerViewModel;
		this.WrapViewerViewModel = wrapViewerViewModel;
		this.ListViewerViewModel = listViewerViewModel;
		this.DetailViewerViewModel = detailViewerViewModel;
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
	public WrapViewerViewModel WrapViewerViewModel {
		get;
	}
	public ListViewerViewModel ListViewerViewModel {
		get;
	}
	public DetailViewerViewModel DetailViewerViewModel {
		get;
	}
}
