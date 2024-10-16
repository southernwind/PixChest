using PixChest.FileTypes.Base.Views;
using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class DetailViewerViewModel : ViewerPaneViewModelBase {
	public DetailViewerViewModel(MediaContentLibraryViewModel mediaContentLibraryViewModel, FilesManager filesManager) : base ("Detail", filesManager){
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
