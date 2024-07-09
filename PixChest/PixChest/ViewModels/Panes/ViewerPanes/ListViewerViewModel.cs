using PixChest.Models.Files;
namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class ListViewerViewModel : ViewerPaneViewModelBase {
	public ReactiveCommand<Unit> ReloadCommand {
		get;
	} = new();

	public ListViewerViewModel(MediaContentLibrary mediaContentLibrary) : base (mediaContentLibrary, "List"){
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		}).AddTo(this.CompositeDisposable);
	}
}
