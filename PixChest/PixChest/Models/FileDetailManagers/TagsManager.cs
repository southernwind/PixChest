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

	public Reactive.Bindings.ReactiveCollection<TagCategory> TagCategories {
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
			var targetCategory = this.TagCategories.MinBy(x => x.TagCategoryId)!;
			tag = new Tag {
				TagCategoryId = targetCategory.TagCategoryId,
				TagName = tagName,
				Detail = detail,
				TagAliases = [],
				TagCategory = targetCategory
			};
			await this._db.AddAsync(tag);
			await this._db.SaveChangesAsync();
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

	public async Task UpdateTag(int tagId, int tagCategoryId, string tagName, string detail, IEnumerable<TagAlias> aliases) {
		using var transaction = this._db.Database.BeginTransaction();
		var tag = this._db.Tags.First(x => x.TagId == tagId);
		tag.TagCategoryId = tagCategoryId;
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

	public async Task UpdateTagCategoryAsync(int tagCategoryId, string tagCategoryName, string detail) {
		using var transaction = this._db.Database.BeginTransaction();
		var tagCategory = this._db.TagCategories.FirstOrDefault(x => x.TagCategoryId == tagCategoryId);
		if (tagCategory != null) {
			tagCategory.TagCategoryName = tagCategoryName;
			tagCategory.Detail = detail;
			this._db.TagCategories.Update(tagCategory);
		} else {
			tagCategory = new TagCategory() {
				TagCategoryId = tagCategoryId,
				TagCategoryName = tagCategoryName,
				Detail = detail,
				Tags = []
			};
			this._db.TagCategories.Add(tagCategory);
		}
		await transaction.CommitAsync();
		await this._db.SaveChangesAsync();
	}

	public async Task Load() {
		this.TagCategories.Clear();
		this.TagsWithKanaRomajiAliases.Clear();
		var tagCategories = await this._db.TagCategories.Include(x => x.Tags).ThenInclude(x => x.TagAliases).ToArrayAsync();
		foreach (var tag in tagCategories.SelectMany(x => x.Tags)) {
			var aliases = tag.TagAliases.Select(x => (x.Alias, x.Ruby)).Concat([(Alias: tag.TagName, Ruby: null)]);
			var newTag = new TagWithRomaji() {
				TagCategory = tag.TagCategory,
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
		this.TagCategories.AddRange(tagCategories);
	}
}
