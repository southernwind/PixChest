using System.Collections.Generic;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Models.FileDetailManagers.Objects;
using PixChest.Models.Files.FileTypes.Base;

namespace PixChest.Models.FileDetailManagers;

[AddSingleton]
public class TagsManager(PixChestDbContext dbContext) {
	private readonly PixChestDbContext _db = dbContext;

	public Reactive.Bindings.ReactiveCollection<Tag> Tags {
		get;
	} = [];

	public Reactive.Bindings.ReactiveCollection<TagWithRomaji> TagsWithKanaRomajiAliases {
		get;
	} = [];

	public async Task AddTag(FileModel[] fileModels, string tagName, string detail = "") {
		var target = fileModels.Where(x => !x.Tags.Any(t => t == tagName)).ToArray();
		using var transaction = this._db.Database.BeginTransaction();
		var tag = await this._db.Tags.FirstOrDefaultAsync(x => x.TagName == tagName);
		if(tag == null) {
			tag = new Tag {
				TagName = tagName,
				Detail = detail,
				TagAliases = []
			};
			await this._db.AddAsync(tag);
			await this._db.SaveChangesAsync();
			this.Tags.Add(tag);
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

	public async Task UpdateTag(int tagId, string tagName, string detail, IEnumerable<TagAlias> aliases) {
		using var transaction = this._db.Database.BeginTransaction();
		var tag = this._db.Tags.First(x => x.TagId == tagId);
		tag.TagName = tagName;
		tag.Detail = detail;
		this._db.Tags.Update(tag);

		this._db.TagAliases.RemoveRange(this._db.TagAliases.Where(x => x.TagId == tagId));
		this._db.TagAliases.AddRange(aliases.Select((x,i) => new TagAlias {
			TagId = tagId,
			TagAliasId = i,
			Alias = x.Alias,
			Ruby = x.Ruby
		}));

		await transaction.CommitAsync();
		await this._db.SaveChangesAsync();
	}

	public async Task Load() {
		this.Tags.Clear();
		this.TagsWithKanaRomajiAliases.Clear();
		var tags = await this._db.Tags.Include(x => x.TagAliases).ToArrayAsync();
		foreach (var tag in tags) {
			var aliases = tag.TagAliases.Select(x => (x.Alias, x.Ruby)).Concat([(Alias: tag.TagName, Ruby: null)]);
			var newTag = new TagWithRomaji() {
				TagName = tag.TagName,
				Detail = tag.Detail,
				TagAliases = aliases.Select(x => new TagAliasWithRomaji() {
					Alias = x.Alias,
					Ruby = x.Ruby,
					Romaji = (x.Ruby ?? x.Alias.KatakanaToHiragana()).HiraganaToRomaji()
				}).ToList()
			};
			this.TagsWithKanaRomajiAliases.Add(newTag);
		}
		this.Tags.AddRange(tags);
	}
}
