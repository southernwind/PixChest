using PixChest.Composition.Bases;
using PixChest.Models.FolderManager;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.FolderManager;
public class FolderManagerWindowViewModel: ViewModelBase {
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

	public FolderManagerWindowViewModel(FolderManagerModel folderManager) {
		this._folderManager = folderManager;
		this.Folders = this._folderManager.Folders.ToReadOnlyReactiveCollection(x => new FolderViewModel(x)).AddTo(this.CompositeDisposable);
		this.AddFolderCommand.Subscribe(x => this._folderManager.AddFolder(x)).AddTo(this.CompositeDisposable);
		this.RemoveFolderCommand.Subscribe(x => this._folderManager.RemoveFolder(x.GetModel())).AddTo(this.CompositeDisposable);
	}
}
