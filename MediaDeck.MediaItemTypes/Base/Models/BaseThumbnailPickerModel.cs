using System.IO;
using System.Threading.Tasks;

using MediaDeck.Composition.Database;
using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;

using Microsoft.Extensions.Logging;

namespace MediaDeck.MediaItemTypes.Base.Models;

[Inject(InjectServiceLifetime.Transient)]
public class BaseThumbnailPickerModel(IDbContextFactory<MediaDeckDbContext> dbFactory, ILogger<BaseThumbnailPickerModel> logger, IFilePathService filePathService) {
	private readonly IDbContextFactory<MediaDeckDbContext> _dbFactory = dbFactory;
	private readonly IFilePathService _filePathService = filePathService;

	public async Task UpdateThumbnailAsync(IMediaItemModel fileModel, byte[] thumbnail, int size) {
		var thumbRelativePath = fileModel.ThumbnailFilePath != null
			? Path.GetRelativePath(Path.Combine(this._filePathService.GetThumbnailAbsoluteFilePath(string.Empty, fileModel.ThumbnailSize)), fileModel.ThumbnailFilePath)
			: this._filePathService.GetThumbnailRelativeFilePath();

		var thumbPath = this._filePathService.GetThumbnailAbsoluteFilePath(thumbRelativePath, size);
		new FileInfo(thumbPath).Directory?.Create();
		await File.WriteAllBytesAsync(thumbPath, thumbnail);

		fileModel.ThumbnailSize = size;
		fileModel.ThumbnailFilePath = thumbPath;

		await using var db = await this._dbFactory.CreateDbContextAsync();
		using var transaction = await db.Database.BeginTransactionAsync();
		var mf = await db.MediaItems.FirstAsync(x => x.MediaItemId == fileModel.Id);
		mf.ThumbnailFileName = thumbRelativePath;
		mf.ThumbnailSize = size;
		db.MediaItems.Update(mf);

		await db.SaveChangesAsync();
		await transaction.CommitAsync();
	}

	public async Task<byte[]?> LoadThumbnailAsync(IMediaItemModel fileModel) {
		if (fileModel.ThumbnailFilePath is not { } path) {
			return null;
		}
		try {
			return await File.ReadAllBytesAsync(path);
		} catch (Exception ex) {
			logger.LogError(ex, "Failed to load thumbnail for file {FileId} at path {ThumbnailPath}", fileModel.Id, path);
			return null;
		}
	}
}