using MediaDeck.Composition.Database;
using MediaDeck.Composition.Interfaces.MediaItemTypes;

namespace MediaDeck.Core.Services.MediaItemMetadataUpdator;

/// <summary>
/// メディアアイテムのメタデータを更新するサービス。
/// </summary>
[Inject(InjectServiceLifetime.Transient)]
public class MediaItemMetadataUpdatorService {
	private readonly IDbContextFactory<MediaDeckDbContext> _dbFactory;
	private readonly IMediaItemTypeService _mediaItemTypeService;

	/// <summary>
	/// 更新対象の総数。
	/// </summary>
	public ReactiveProperty<long> TargetCount {
		get;
	} = new();

	/// <summary>
	/// 更新完了数。
	/// </summary>
	public ReactiveProperty<long> CompletedCount {
		get;
	} = new();

	public MediaItemMetadataUpdatorService(IDbContextFactory<MediaDeckDbContext> dbFactory, IMediaItemTypeService mediaItemTypeService) {
		this._dbFactory = dbFactory;
		this._mediaItemTypeService = mediaItemTypeService;
	}

	/// <summary>
	/// 全てのメディアアイテムのメタデータを更新する。
	/// </summary>
	/// <param name="ct">キャンセル・トークン</param>
	/// <returns>タスク</returns>
	public async Task UpdateMetadataAsync(CancellationToken ct = default) {
		List<long> targetIds;
		await using (var db = await this._dbFactory.CreateDbContextAsync(ct)) {
			targetIds = await db.MediaItems.Select(x => x.MediaItemId).ToListAsync(ct);
		}

		this.TargetCount.Value = targetIds.Count;
		this.CompletedCount.Value = 0;

		var operators = this._mediaItemTypeService.CreateMediaItemOperators().ToDictionary(x => x.TargetMediaType);

		// 50件ずつ処理する
		foreach (var chunk in targetIds.Chunk(50)) {
			if (ct.IsCancellationRequested) {
				return;
			}

			await using (var db = await this._dbFactory.CreateDbContextAsync(ct)) {
				using var transaction = await db.Database.BeginTransactionAsync(ct);

				var items = await this._mediaItemTypeService.IncludeTables(db.MediaItems)
					.Where(x => chunk.Contains(x.MediaItemId))
					.ToListAsync(ct);

				foreach (var file in items) {
					if (operators.TryGetValue(file.MediaType, out var op)) {
						await op.UpdateMetadata(file);
					}
					this.CompletedCount.Value++;
				}

				await db.SaveChangesAsync(ct);
				await transaction.CommitAsync(ct);
			}
		}
	}
}