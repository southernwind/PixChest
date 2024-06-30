using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.Models.FolderManager;

namespace PixChest.ViewModels.FolderManager;

[AddTransient]
public class FolderManagerViewModel: ViewModelBase {
	private readonly FolderManagerModel _folderManager;

	public Reactive.Bindings.ReadOnlyReactiveCollection<FolderViewModel> Folders {
		get;
	}

	public ReactiveCommand<string> AddFolderCommand {
		get;
	} = new();
	public ReactiveCommand<FolderViewModel> RemoveFolderCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> ScanCommand {
		get;
	} = new();

	public BindableReactiveProperty<long> QueueCount {
		get;
	}

	public BindableReactiveProperty<bool> IsScanning {
		get;
	}

	public FolderManagerViewModel(FolderManagerModel folderManager, FileRegistrar fileRegistrar) {
		this._folderManager = folderManager;
		this.Folders = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this._folderManager.Folders, x => new FolderViewModel(x));
		this.AddFolderCommand.Subscribe(x => this._folderManager.AddFolder(x)).AddTo(this.CompositeDisposable);
		this.RemoveFolderCommand.Subscribe(x => this._folderManager.RemoveFolder(x.GetModel())).AddTo(this.CompositeDisposable);
		this.ScanCommand.Subscribe(async x => await this._folderManager.Scan()).AddTo(this.CompositeDisposable);
		this.QueueCount = fileRegistrar.QueueCount.ToBindableReactiveProperty();
		this.IsScanning = fileRegistrar.QueueCount.Select(x => x > 0).ToBindableReactiveProperty();
	}
}
