using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Utils.Objects;

using System.IO;
using System.Threading.Tasks;

namespace PixChest.Models.FolderManager; 
public class FolderModel(string folderPath, PixChestDbContext dbContext) {
	private readonly PixChestDbContext _db = dbContext;
	private readonly object _lock = new();
	public string FolderPath {
		get;
	} = folderPath;

	public ProgressCount ScanProgress {
		get;
	} = new();

	public async Task Scan() {
		this.ScanProgress.InProgress.Value = true;
		var files = Directory.EnumerateFiles(this.FolderPath, ".", SearchOption.AllDirectories);
		this.ScanProgress.Total.Value = files.Count();
		using var transaction = this._db.Database.BeginTransaction();
		foreach (var file in files) {
			var isExists = await this._db.MediaFiles.AnyAsync(x => x.FilePath == file);
			if (isExists) {
				continue;
			}

			this._db.MediaFiles.Add(new MediaFile {
				DirectoryPath = Path.GetDirectoryName(file)!,
				FilePath = file
			});
		}
		this._db.SaveChanges();
		await transaction.CommitAsync();
	}
}
