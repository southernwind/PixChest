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
		// 1. 全アイテムの情報を一気にメモリに読み込む
		List<MediaItemBasicInfo> items;
		await using (var db = await this._dbFactory.CreateDbContextAsync(ct)) {
			items = await db.MediaItems
				.Select(x => new MediaItemBasicInfo(
					x.MediaItemId,
					x.FilePath,
					x.MediaType,
					x.IsExists,
					x.FileSize,
					x.CreationTime,
					x.ModifiedTime,
					x.LastAccessTime,
					x.PreHashUpdatedTime
				))
				.AsNoTracking()
				.ToListAsync(ct);
		}

		this.TargetCount.Value = items.Count;
		this.CompletedCount.Value = 0;

		var updates = new List<FileStatusUpdateInfo>();

		// 2. 状態チェックフェーズ (DBアクセスなし、ディスクI/Oのみ)
		foreach (var item in items) {
			if (ct.IsCancellationRequested) {
				return;
			}

			var mediaItemTypeProvider = this._mediaItemTypeService.GetMediaItemTypeProvider(item.MediaType);
			var pathStatus = mediaItemTypeProvider.GetPathStatus(item.FilePath);

			if (
				item.IsExists == pathStatus.Exists &&
				(!item.IsExists ||
					(
						item.FileSize == pathStatus.FileSize &&
						item.CreationTime == pathStatus.CreationTime &&
						item.ModifiedTime == pathStatus.ModifiedTime &&
						item.LastAccessTime == pathStatus.LastAccessTime &&
						item.PreHashUpdatedTime != null &&
						item.PreHashUpdatedTime >= pathStatus.ModifiedTime
					)
				)
			) {
				this.CompletedCount.Value++;
				continue;
			}

			var needsHashUpdate = pathStatus.Exists && (item.PreHashUpdatedTime == null || item.PreHashUpdatedTime < pathStatus.ModifiedTime) && item.MediaType != MediaType.FolderGroup;

			if (needsHashUpdate) {
				this._fileHashUpdatorService.EnqueueHashUpdate(item.MediaItemId);
			}

			updates.Add(new FileStatusUpdateInfo(
				item.MediaItemId,
				pathStatus.Exists,
				pathStatus.Exists ? pathStatus.FileSize : item.FileSize,
				pathStatus.Exists ? pathStatus.CreationTime : item.CreationTime,
				pathStatus.Exists ? pathStatus.ModifiedTime : item.ModifiedTime,
				pathStatus.Exists ? pathStatus.LastAccessTime : item.LastAccessTime
			));
			this.CompletedCount.Value++;
		}

		// 3. 更新フェーズ (ExecuteUpdate を使用してフェッチなしで直接更新)
		if (updates.Any()) {
			foreach (var updateChunk in updates.Chunk(100)) {
				if (ct.IsCancellationRequested) {
					return;
				}

				await using var db = await this._dbFactory.CreateDbContextAsync(ct);
				using var transaction = await db.Database.BeginTransactionAsync(ct);
				foreach (var info in updateChunk) {
					if (info.IsExists) {
						await db.MediaItems
							.Where(x => x.MediaItemId == info.MediaItemId)
							.ExecuteUpdateAsync(s => s
								.SetProperty(m => m.IsExists, info.IsExists)
								.SetProperty(m => m.FileSize, info.FileSize)
								.SetProperty(m => m.CreationTime, info.CreationTime)
								.SetProperty(m => m.ModifiedTime, info.ModifiedTime)
								.SetProperty(m => m.LastAccessTime, info.LastAccessTime),
							ct);
					} else {
						await db.MediaItems
							.Where(x => x.MediaItemId == info.MediaItemId)
							.ExecuteUpdateAsync(s => s
								.SetProperty(m => m.IsExists, info.IsExists),
							ct);
					}
				}
				await transaction.CommitAsync(ct);
			}
		}


		// PreHash更新がなかった場合もFullHashのチェックを行う
		await this._fileHashUpdatorService.CheckAndEnqueueFullHashUpdatesAsync(ct);
	}

	private record FileStatusUpdateInfo(
		long MediaItemId,
		bool IsExists,
		long FileSize,
		DateTime CreationTime,
		DateTime ModifiedTime,
		DateTime LastAccessTime
	);

	private record MediaItemBasicInfo(
		long MediaItemId,
		string FilePath,
		MediaType MediaType,
		bool IsExists,
		long FileSize,
		DateTime CreationTime,
		DateTime ModifiedTime,
		DateTime LastAccessTime,
		DateTime? PreHashUpdatedTime
	);
}