using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Database;

namespace PixChest.Models.FolderManager;
public class FolderManagerModel(PixChestDbContext dbContext) : ModelBase{
	private readonly PixChestDbContext _db = dbContext;
	public ReactiveCollection<FolderModel> Folders {
		get;
	} = [];

	public void AddFolder(string folderPath) {
		this.Folders.Add(new FolderModel(folderPath, this._db));
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
