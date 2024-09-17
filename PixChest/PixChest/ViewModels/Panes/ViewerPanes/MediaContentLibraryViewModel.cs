using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddSingleton]
public class MediaContentLibraryViewModel : ViewModelBase {
	public MediaContentLibraryViewModel(MediaContentLibrary mediaContentLibrary, TagsManager tagsManager) {
		this.Files = mediaContentLibrary.Files.CreateView(FileTypeUtility.CreateFileViewModel).ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.SearchAsync();
		}).AddTo(this.CompositeDisposable);
		this.SearchWord.Subscribe(x => {
			mediaContentLibrary.Word = x;
		});
	}

	public INotifyCollectionChangedSynchronizedViewList<IFileViewModel> Files {
		get;
	}

	public BindableReactiveProperty<string> SearchWord {
		get;
	} = new();

	public BindableReactiveProperty<IFileViewModel> SelectedFile {
		get;
	} = new();

	public BindableReactiveProperty<IFileViewModel[]> SelectedFiles {
		get;
	} = new();

	public ReactiveCommand<Unit> ReloadCommand {
		get;
	} = new();
}
