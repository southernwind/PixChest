using MediaDeck.Composition.Database;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.MediaItemTypes;

namespace MediaDeck.Core.Services.FileStatusUpdator;

[Inject(InjectServiceLifetime.Transient)]
public class FileStatusUpdatorService {
	public FileStatusUpdatorService(IDbContextFactory<MediaDeckDbContext> dbFactory, IFileHashUpdatorService fileHashUpdatorService, IMediaItemTypeService mediaItemTypeService) {
		this._dbFactory = dbFactory;
		this._fileHashUpdatorService = fileHashUpdatorService;
		this._mediaItemTypeService = mediaItemTypeService;
	}

	private readonly IDbContextFactory<MediaDeckDbContext> _dbFactory;
	private readonly IFileHashUpdatorService _fileHashUpdatorService;
	private readonly IMediaItemTypeService _mediaItemTypeService;

	public ReactiveProperty<long> TargetCount {
		get;
	} = new();

	public ReactiveProperty<long> CompletedCount {
		get;
	} = new();

	public async Task UpdateFileInfo(CancellationToken ct = default) {
		List<long> targetIds;
		await using (var db = await this._dbFactory.CreateDbContextAsync(ct)) {
			targetIds = await db.MediaItems.Select(x => x.MediaItemId).ToListAsync(ct);
		}
		this.TargetCount.Value = targetIds.Count;
		this.CompletedCount.Value = 0;

		foreach (var chunk in targetIds.Chunk(50)) {
			if (ct.IsCancellationRequested) {
				return;
			}

			await using (var db = await this._dbFactory.CreateDbContextAsync(ct)) {
				using var transaction = await db.Database.BeginTransactionAsync(ct);

				var items = await db.MediaItems
					.Where(x => chunk.Contains(x.MediaItemId))
					.ToListAsync(ct);

				bool hasUpdate = false;
				foreach (var file in items) {
					this.CompletedCount.Value++;
					var mediaItemTypeProvider = this._mediaItemTypeService.GetMediaItemTypeProvider(file.MediaType);
					var pathStatus = mediaItemTypeProvider.GetPathStatus(file.FilePath);
					if (
						file.IsExists == pathStatus.Exists &&
						(!file.IsExists ||
							(
								file.FileSize == pathStatus.FileSize &&
								file.CreationTime == pathStatus.CreationTime &&
								file.ModifiedTime == pathStatus.ModifiedTime &&
								file.LastAccessTime == pathStatus.LastAccessTime &&
								file.PreHashUpdatedTime != null &&
								file.PreHashUpdatedTime >= pathStatus.ModifiedTime
							)
						)
					) {
						continue;
					}
					var needsHashUpdate = pathStatus.Exists && (file.PreHashUpdatedTime == null || file.PreHashUpdatedTime < pathStatus.ModifiedTime) && file.MediaType != MediaType.FolderGroup;

					file.IsExists = pathStatus.Exists;

					if (file.IsExists) {
						if (needsHashUpdate) {
							this._fileHashUpdatorService.EnqueueHashUpdate(file.MediaItemId);
						}
						file.FileSize = pathStatus.FileSize;
						file.CreationTime = pathStatus.CreationTime;
						file.ModifiedTime = pathStatus.ModifiedTime;
						file.LastAccessTime = pathStatus.LastAccessTime;
					}
					hasUpdate = true;
				}

				if (hasUpdate) {
					await db.SaveChangesAsync(ct);
				}
				await transaction.CommitAsync(ct);
			}
		}

		// PreHash更新がなかった場合もFullHashのチェックを行う
		await this._fileHashUpdatorService.CheckAndEnqueueFullHashUpdatesAsync(ct);
	}
}