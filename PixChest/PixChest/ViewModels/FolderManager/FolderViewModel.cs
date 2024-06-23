using PixChest.Composition.Bases;
using PixChest.Models.FolderManager;
using PixChest.Utils.Objects;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.FolderManager;
public class FolderViewModel(FolderModel folderModel) : ViewModelBase {
	private readonly FolderModel _folderModel = folderModel;
	public string FolderPath {
		get {
			return this._folderModel.FolderPath;
		}
	}

	public ProgressCount ScanProgress {
		get {
			return  this._folderModel.ScanProgress;
		}
	}

	public FolderModel GetModel() {
		return this._folderModel;
	}

}
