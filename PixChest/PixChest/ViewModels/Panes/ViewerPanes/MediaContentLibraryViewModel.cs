using PixChest.Composition.Bases;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.Files;
using PixChest.Models.Files.FileTypes.Base;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddSingleton]
public class MediaContentLibraryViewModel : ViewModelBase {
	public MediaContentLibraryViewModel(MediaContentLibrary mediaContentLibrary, TagsManager tagsManager) {
		this.Files = mediaContentLibrary.Files.CreateView(x => new FileViewModel(x)).ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.SearchAsync();
		}).AddTo(this.CompositeDisposable);
		this.SearchWord.Subscribe(x => {
			mediaContentLibrary.Word = x;
		});
	}

	public INotifyCollectionChangedSynchronizedViewList<FileViewModel> Files {
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
