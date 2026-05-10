using MediaDeck.Common.Base;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Stores.Config.Model.Objects;
using MediaDeck.Core.Models.Files;
using MediaDeck.Core.Stores.Config;

namespace MediaDeck.Core.Models.FolderManager;

[Inject(InjectServiceLifetime.Transient)]
public class FolderManagerModel : ModelBase {
	private readonly FileRegistrar _fileRegistrar;
	private readonly FolderManagerConfigModel _folderManagerConfig;
	private readonly IConfigStore _configStore;

	public FolderManagerModel(FileRegistrar fileRegistrar, FolderManagerConfigModel folderManagerConfig,IConfigStore configStore) {
		this._fileRegistrar = fileRegistrar;
		this._folderManagerConfig = folderManagerConfig;
		this.Folders = this._folderManagerConfig.Folders;
		this._configStore = configStore;
	}

	public ObservableList<FolderModel> Folders {
		get;
	} = [];

	public void AddFolder(string folderPath) {
		this._folderManagerConfig.Folders.Add(new FolderModel() { FolderPath = folderPath });
		this._configStore.Save();
	}

	public void RemoveFolder(FolderModel folder) {
		this._folderManagerConfig.Folders.Remove(folder);
		this._configStore.Save();
	}

	public async Task Scan() {
		foreach (var folder in this.Folders.ToArray()) {
			await this._fileRegistrar.ScanFolderAsync(folder);
		}
		this._configStore.Save();
	}

	public async Task ScanFolder(FolderModel folder) {
		await this._fileRegistrar.ScanFolderAsync(folder);
		this._configStore.Save();
	}
}