using System.Collections.Generic;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.Files.Filter;
using PixChest.Models.Files.SearchConditions;
using PixChest.Models.Files.Sort;
using PixChest.Utils.Constants;

namespace PixChest.Models.Files.Loaders;

[AddTransient]
public class FilesLoader(PixChestDbContext dbContext, SortSelector sortSelector,FilterSelector filterSetter) {
	protected FilterSelector FilterSetter = filterSetter;
	protected SortSelector SortSelector = sortSelector;

	public async Task<IEnumerable<IFileModel>> Load(IEnumerable<ISearchCondition> searchConditions) {
		using var lockObject = await LockObjectConstants.DbLock.LockAsync();
		var files =
			(await dbContext
				.MediaFiles
				.Where(searchConditions)
				.Include(mf => mf.MediaFileTags)
				.ThenInclude(mft => mft.Tag)
				.ThenInclude(t => t.TagCategory)
				.Include(mf => mf.MediaFileTags)
				.ThenInclude(mft => mft.Tag)
				.ThenInclude(t => t.TagAliases)
				.Include(mf => mf.Position)
				.IncludeTables()
				.ToArrayAsync())
				.Select(FileTypeUtility.CreateFileModelFromRecord)
				.Where(searchConditions)
				.Where(this.FilterSetter);

		return this.SortSelector.SetSortConditions(files);
	}
}

