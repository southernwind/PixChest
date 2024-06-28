using System.Reactive.Linq;

using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.Models.FolderManager;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.FolderManager;

[AddTransient]
public class FolderManagerViewModel: ViewModelBase {
	private readonly FolderManagerModel _folderManager;

	public ReadOnlyReactiveCollection<FolderViewModel> Folders {
		get;
	}

	public ReactiveCommand<string> AddFolderCommand {
		get;
	} = new();
	public ReactiveCommand<FolderViewModel> RemoveFolderCommand {
		get;
	} = new();

	public ReactiveCommand ScanCommand {
		get;
	} = new();

	public IReadOnlyReactiveProperty<long> QueueCount {
		get;
	}

	public IReadOnlyReactiveProperty<bool> IsScanning {
		get;
	}

	public FolderManagerViewModel(FolderManagerModel folderManager, FileRegistrar fileRegistrar) {
		this._folderManager = folderManager;
		this.Folders = this._folderManager.Folders.ToReadOnlyReactiveCollection(x => new FolderViewModel(x)).AddTo(this.CompositeDisposable);
		this.AddFolderCommand.Subscribe(x => this._folderManager.AddFolder(x)).AddTo(this.CompositeDisposable);
		this.RemoveFolderCommand.Subscribe(x => this._folderManager.RemoveFolder(x.GetModel())).AddTo(this.CompositeDisposable);
		this.ScanCommand.Subscribe(async x => await this._folderManager.Scan()).AddTo(this.CompositeDisposable);
		this.QueueCount = fileRegistrar.QueueCount.ToReadOnlyReactiveProperty().AddTo(this.CompositeDisposable);
		this.IsScanning = fileRegistrar.QueueCount.Select(x => x > 0).ToReadOnlyReactiveProperty().AddTo(this.CompositeDisposable);
	}
}
