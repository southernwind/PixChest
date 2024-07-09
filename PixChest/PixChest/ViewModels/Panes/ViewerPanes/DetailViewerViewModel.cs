using PixChest.Models.Files;
namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class WrapViewerViewModel : ViewerPaneViewModelBase {
	public ReactiveCommand<Unit> ReloadCommand {
		get;
	} = new();

	public WrapViewerViewModel(MediaContentLibrary mediaContentLibrary) : base (mediaContentLibrary, "Wrap"){
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		}).AddTo(this.CompositeDisposable);
	}
}
