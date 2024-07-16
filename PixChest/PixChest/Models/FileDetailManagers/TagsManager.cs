using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Models.Files;

namespace PixChest.Models.FileDetailManagers;

[AddSingleton]
public class TagsManager(PixChestDbContext dbContext) {
	private readonly PixChestDbContext _db = dbContext;

	public Reactive.Bindings.ReactiveCollection<Tag> TagCandidates {
		get;
	} = [];

	public async Task AddTag(FileModel[] fileModels, string tagName) {
		var target = fileModels.Where(x => !x.Tags.Any(t => t == tagName)).ToArray();
		using var transaction = this._db.Database.BeginTransaction();
		var tag = await this._db.Tags.FirstOrDefaultAsync(x => x.TagName == tagName);
		if(tag == null) {
			tag = new Tag {
				TagName = tagName
			};
			await this._db.AddAsync(tag);
			await this._db.SaveChangesAsync();
			this.TagCandidates.Add(tag);
		}
		await this._db.MediaFileTags.AddRangeAsync(target.Select(x => new MediaFileTag {
			MediaFileId = x.Id,
			TagId = tag.TagId
		}));
		await this._db.SaveChangesAsync();
		foreach(var file in target) {
			file.Tags.Add(tagName);
		}
		await transaction.CommitAsync();
	}

	public async Task RemoveTag(FileModel[] fileModels, string tagName) {
		var ids = fileModels.Select(x => x.Id);
		using var transaction = this._db.Database.BeginTransaction();
		var rel =
			await
			this._db
				.MediaFileTags
				.Where(x => ids.Contains(x.MediaFileId) && x.Tag.TagName == tagName)
				.ToArrayAsync();
		if (!rel.IsEmpty()) {
			this._db.MediaFileTags.RemoveRange(rel);
			await this._db.SaveChangesAsync();
			foreach (var file in fileModels) {
				file.Tags.Remove(tagName);
			}
			await transaction.CommitAsync();
		}
	}

	public async Task Load() {
		this.TagCandidates.Clear();
		var tags = await this._db.Tags.ToArrayAsync();
		this.TagCandidates.AddRange(tags);
	}
}
