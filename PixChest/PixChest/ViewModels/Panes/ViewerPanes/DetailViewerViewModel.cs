using PixChest.FileTypes.Base.Views;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class DetailViewerViewModel : ViewerPaneViewModelBase {
	public DetailViewerViewModel(MediaContentLibraryViewModel mediaContentLibraryViewModel) : base ("Detail"){
		this.MediaContentLibraryViewModel = mediaContentLibraryViewModel;
		mediaContentLibraryViewModel.SelectedFile.Subscribe(x => {
			if (x is not {} vm) {
				this.DetailViewerPreviewControlView.Value = null;
				return;
			}
			this.DetailViewerPreviewControlView.Value = FileTypeUtility.CreateDetailViewerPreviewControlView(vm);
			this.DetailViewerPreviewControlView.Value.DataContext = this;
		});
	}

	public MediaContentLibraryViewModel MediaContentLibraryViewModel {
		get;
	}

	public BindableReactiveProperty<IDetailViewerPreviewControlView?> DetailViewerPreviewControlView {
		get;
	} = new();
}
