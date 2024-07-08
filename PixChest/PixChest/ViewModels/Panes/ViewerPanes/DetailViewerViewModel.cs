using PixChest.Models.Files;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class DetailViewerViewModel : ViewerPaneViewModelBase {
	public ReactiveCommand<Unit> ReloadCommand {
		get;
	} = new();

	public DetailViewerViewModel(MediaContentLibrary mediaContentLibrary) : base (mediaContentLibrary){
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		}).AddTo(this.CompositeDisposable);
	}
}
