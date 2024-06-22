using PixChest.Models.FolderManager;

namespace PixChest.ViewModels.FolderManager;
public class FolderViewModel {
	private readonly FolderModel _folderModel;
	public string FolderPath {
		get {
			return this._folderModel.FolderPath;
		}
	}

	public FolderViewModel(FolderModel folderModel) {
		this._folderModel = folderModel;
	}

	public FolderModel GetModel() {
		return this._folderModel;
	}

}
