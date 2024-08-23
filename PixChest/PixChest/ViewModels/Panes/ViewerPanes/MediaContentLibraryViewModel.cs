using PixChest.Composition.Bases;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.Files;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class MediaContentLibraryViewModel : ViewModelBase {
	public MediaContentLibraryViewModel(MediaContentLibrary mediaContentLibrary, TagsManager tagsManager) {
		this.Files = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(mediaContentLibrary.Files, x => new FileViewModel(x));
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		}).AddTo(this.CompositeDisposable);
		this.SearchWord.Subscribe(x => {
			mediaContentLibrary.Word = x;
		});
	}

	public Reactive.Bindings.ReadOnlyReactiveCollection<FileViewModel> Files {
		get;
	}

	public BindableReactiveProperty<string> SearchWord {
		get;
	} = new();

	public BindableReactiveProperty<FileViewModel> SelectedFile {
		get;
	} = new();

	public BindableReactiveProperty<FileViewModel[]> SelectedFiles {
		get;
	} = new();

	public ReactiveCommand<Unit> ReloadCommand {
		get;
	} = new();
}
