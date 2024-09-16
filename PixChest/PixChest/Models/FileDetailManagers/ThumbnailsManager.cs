using System.IO;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.FileTypes.Base.Models.Interfaces;

namespace PixChest.Models.FileDetailManagers;

[AddSingleton]
public class ThumbnailsManager(PixChestDbContext dbContext) {
	private readonly PixChestDbContext _db = dbContext;

	public async Task UpdateThumbnail(IFileModel fileModel, byte[] thumbnail) {
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

	public async Task<byte[]?> LoadThumbnailAsync(IFileModel fileModel) {
		if (fileModel.ThumbnailFilePath is not { } path) {
			return null;
		}
		return await File.ReadAllBytesAsync(path);
	}
}
