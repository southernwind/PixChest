using Windows.Storage;

namespace PixChest.Models.FolderManager; 
public class FolderModel {
	public string FolderPath {
		get;
	}

	public FolderModel(string folderPath) {
		this.FolderPath = folderPath;
	}
}
