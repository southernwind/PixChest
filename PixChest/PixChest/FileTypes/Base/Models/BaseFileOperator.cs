using System.Threading.Tasks;

using CommunityToolkit.Mvvm.DependencyInjection;

using PixChest.Database;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Base.Models;

public abstract class BaseFileOperator : IFileOperator {
	protected readonly PixChestDbContext _db;

	protected BaseFileOperator() {
		this._db = Ioc.Default.GetRequiredService<PixChestDbContext>();
	}

	public virtual async Task UpdateRateAsync(long mediaFileId, int rate) {
		using var transaction = await this._db.Database.BeginTransactionAsync();
		var file = await this._db.MediaFiles.FirstOrDefaultAsync(x => x.MediaFileId == mediaFileId);
		if (file is not { } mediaFile) {
			return;
		}
		mediaFile.Rate = rate;
		this._db.Update(mediaFile);
		await transaction.CommitAsync();
		await this._db.SaveChangesAsync();
	}

	public virtual async Task IncrementUsageCountAsync(long mediaFileId) {
		using var transaction = await this._db.Database.BeginTransactionAsync();
		var file = await this._db.MediaFiles.FirstOrDefaultAsync(x => x.MediaFileId == mediaFileId);
		if (file is not { } mediaFile) {
			return;
		}
		mediaFile.UsageCount++;
		this._db.Update(mediaFile);
		await transaction.CommitAsync();
		await this._db.SaveChangesAsync();
	}

	public virtual async Task UpdateDescriptionAsync(long mediaFileId, string description) {
		using var transaction = await this._db.Database.BeginTransactionAsync();
		var file = await this._db.MediaFiles.FirstOrDefaultAsync(x => x.MediaFileId == mediaFileId);
		if (file is not { } mediaFile) {
			return;
		}
		mediaFile.Description = description;
		this._db.Update(mediaFile);
		await transaction.CommitAsync();
		await this._db.SaveChangesAsync();
	}


	public abstract MediaType TargetMediaType {
		get;
	}

	public abstract void RegisterFile(string filePath);
}