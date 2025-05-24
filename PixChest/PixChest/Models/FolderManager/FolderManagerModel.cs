using System.IO;
using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.Models.Preferences.CustomStates;

namespace PixChest.Models.FolderManager;

[AddTransient]
public class FolderManagerModel : ModelBase {
	private readonly FileRegistrar _fileRegistrar;
	private readonly FolderManagerStates _folderManagerStates;
	public FolderManagerModel(FileRegistrar fileRegistrar, FolderManagerStates folderManagerStates) {
		this._fileRegistrar = fileRegistrar;
		this._folderManagerStates = folderManagerStates;
		this.Folders = this._folderManagerStates.Folders;
	}

	public ObservableList<FolderModel> Folders {
		get;
	} = [];

	public void AddFolder(string folderPath) {
		this._folderManagerStates.Folders.Add(new FolderModel() { FolderPath = folderPath });
	}

	public void RemoveFolder(FolderModel folder) {
		this._folderManagerStates.Folders.Remove(folder);
	}

	public async Task Scan() {
		foreach (var folder in this.Folders) {
			var files = Directory.EnumerateFiles(folder.FolderPath, "", SearchOption.AllDirectories);
			await Task.Run(() => {
				this._fileRegistrar.RegistrationQueue.EnqueueRange(files.Where(x => x.IsTargetFile()));
			});
		}
	}
}
