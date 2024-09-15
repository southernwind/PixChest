using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.FileTypes.Base.Models;
using PixChest.Models.FileDetailManagers.Objects;
using PixChest.ViewModels.Files;

namespace PixChest.Models.FileDetailManagers;

[AddSingleton]
public class ThumbnailsManager(PixChestDbContext dbContext) {
	private readonly PixChestDbContext _db = dbContext;

	public async Task UpdateThumbnail(BaseFileModel fileModel, byte[] thumbnail) {
		var thumbPath = FilePathUtility.GetThumbnailRelativeFilePath(fileModel.FilePath);
		await File.WriteAllBytesAsync(thumbPath, thumbnail);

		if (fileModel.ThumbnailFilePath == thumbPath) {
			return;
		}
		using var transaction = this._db.Database.BeginTransaction();
		var mf = this._db.MediaFiles.First(x => x.MediaFileId== fileModel.Id);
		mf.ThumbnailFileName = thumbPath;
		this._db.MediaFiles.Update(mf);

		await transaction.CommitAsync();
		await this._db.SaveChangesAsync();
	}

	public async Task<byte[]?> LoadThumbnailAsync(BaseFileModel fileModel) {
		if (fileModel.ThumbnailFilePath is not { } path) {
			return null;
		}
		return await File.ReadAllBytesAsync(path);
	}
}
