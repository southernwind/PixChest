using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Models.Files;

namespace PixChest.Models.FileEditors;

[AddSingleton]
public class TagsManager(PixChestDbContext dbContext) {
	private readonly PixChestDbContext _db = dbContext;

	public Reactive.Bindings.ReactiveCollection<Tag> TagCandidates {
		get;
	} = [];

	public async Task AddTag(FileModel fileModel, string tagName) {
		var tag = new Tag {
			TagName = tagName
		};
		using var transaction = this._db.Database.BeginTransaction();
		if(this._db.Tags.Any(x => x.TagName == tagName)) {
			tag = await this._db.Tags.FirstAsync(x => x.TagName == tagName);
			await this._db.MediaFileTags.AddAsync(new MediaFileTag {
				MediaFileId = fileModel.Id,
				TagId = tag.TagId
			});
		} else {
			await this._db.AddAsync(tag);
			await this._db.SaveChangesAsync();
			this.TagCandidates.Add(tag);
			await this._db.MediaFileTags.AddAsync(new MediaFileTag {
				MediaFileId = fileModel.Id,
				Tag = tag
			});
		}
		await this._db.SaveChangesAsync();
		await transaction.CommitAsync();
	}

	public async Task RemoveTag(FileModel fileModel, string tagName) {
		using var transaction = this._db.Database.BeginTransaction();
		var rel =
			await
			this._db
				.MediaFileTags
				.FirstOrDefaultAsync(x => x.MediaFileId == fileModel.Id && x.Tag.TagName == tagName);
		if (rel != null) {
			this._db.MediaFileTags.Remove(rel);
			await this._db.SaveChangesAsync();
			await transaction.CommitAsync();
		}
	}

	public async Task Load() {
		this.TagCandidates.Clear();
		var tags = await this._db.Tags.ToArrayAsync();
		this.TagCandidates.AddRange(tags);
	}
}
