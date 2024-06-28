using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Models.Files;

namespace PixChest.Models.FolderManager;

[AddTransient]
public class FolderManagerModel(FileRegistrar fileRegistrar) : ModelBase{
	private readonly FileRegistrar _fileRegistrar = fileRegistrar;
	public ReactiveCollection<FolderModel> Folders {
		get;
	} = [];

	public void AddFolder(string folderPath) {
		this.Folders.Add(new FolderModel(folderPath, this._fileRegistrar));
	}

	public void RemoveFolder(FolderModel folder) {
		this.Folders.Remove(folder);
	}

	public async Task Scan() {
		foreach (var folder in this.Folders) {
			await folder.Scan();
		}
	}
}
