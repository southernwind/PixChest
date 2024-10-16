using System.Threading.Tasks;

using PixChest.Database;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Utils.Constants;

namespace PixChest.Models.FileDetailManagers;

[AddTransient]
public class FilesManager {
	public FilesManager(PixChestDbContext db) {
		this._db = db;
	}
	private readonly PixChestDbContext _db;

	public async Task RemoveFileAsync(IFileModel fileModel) {
		using var lockObject = await LockObjectConstants.DbLock.LockAsync();
		using var transaction = await this._db.Database.BeginTransactionAsync();
		var targetFile =
			await this._db
				.MediaFiles
				.FirstOrDefaultAsync(x => x.MediaFileId == fileModel.Id);

		if(targetFile == null) {
			return;
		}
		this._db.MediaFiles.Remove(targetFile);
		await this._db.SaveChangesAsync();
		await transaction.CommitAsync();
	}
}
