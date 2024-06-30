using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class DetailViewerViewModel :ViewModelBase {
	public Reactive.Bindings.ReadOnlyReactiveCollection<FileViewModel> Files {
		get;
	}

	public ReactiveCommand<Unit> ReloadCommand {
		get;
	} = new();

	public DetailViewerViewModel(MediaContentLibrary mediaContentLibrary) {
		this.Files = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(mediaContentLibrary.Files, x => new FileViewModel(x));
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		}).AddTo(this.CompositeDisposable);
	}
}
