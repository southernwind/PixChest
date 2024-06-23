using System.IO;
using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Database;

namespace PixChest.Models.Files; 
public class MediaContentLibrary(PixChestDbContext dbContext):ModelBase {
	private readonly PixChestDbContext _db = dbContext;

	public ReactiveCollection<FileModel> Files {
		get;
	} = [];

	public async Task Search() {
		var files = await this._db.MediaFiles.ToListAsync();
		this.Files.Clear();
		this.Files.AddRangeOnScheduler(files.Select(x => new FileModel(x.FilePath)));
	}

}
