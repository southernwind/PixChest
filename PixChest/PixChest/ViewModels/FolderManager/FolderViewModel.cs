using PixChest.Composition.Bases;
using PixChest.Models.FolderManager;

namespace PixChest.ViewModels.FolderManager;
public class FolderViewModel(FolderModel folderModel) : ViewModelBase {
	private readonly FolderModel _folderModel = folderModel;
	public string FolderPath {
		get {
			return this._folderModel.FolderPath;
		}
	}

	public FolderModel GetModel() {
		return this._folderModel;
	}

}
