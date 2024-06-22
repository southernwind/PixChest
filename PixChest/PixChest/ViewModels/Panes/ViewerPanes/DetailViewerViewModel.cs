using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.ViewModels.Files;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Panes.ViewerPanes;
public class DetailViewerViewModel :ViewModelBase {
	public ReadOnlyReactiveCollection<FileViewModel> Files {
		get;
	}

	public AsyncReactiveCommand ReloadCommand {
		get;
	} = new();

	public DetailViewerViewModel(MediaContentLibrary mediaContentLibrary) {
		this.Files = mediaContentLibrary.Files.ToReadOnlyReactiveCollection(x => new FileViewModel(x)).AddTo(this.CompositeDisposable);
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		}).AddTo(this.CompositeDisposable);
	}
}
