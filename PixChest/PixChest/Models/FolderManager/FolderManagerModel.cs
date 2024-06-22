
using PixChest.Composition.Bases;

namespace PixChest.Models.FolderManager;
public class FolderManagerModel : ModelBase{
	public ReactiveCollection<FolderModel> Folders {
		get;
	} = [];

	public void AddFolder(string folderPath) {
		this.Folders.Add(new FolderModel(folderPath));
	}

	public void RemoveFolder(FolderModel folder) {
		this.Folders.Remove(folder);
	}
}
