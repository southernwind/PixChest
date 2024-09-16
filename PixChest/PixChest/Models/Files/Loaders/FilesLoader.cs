using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.Files.Filter;
using PixChest.Models.Files.Sort;
using PixChest.Models.Repositories;

namespace PixChest.Models.Files.Loaders;

public abstract class FilesLoader(PixChestDbContext dbContext, SortSelector sortSelector,FilterSelector filterSetter, RepositorySelector repositorySelector) {
	protected FilterSelector FilterSetter = filterSetter;
	protected SortSelector SortSelector = sortSelector;
	private readonly RepositorySelector repositorySelector = repositorySelector;

	public async Task<IEnumerable<IFileModel>> Load() {
		var files =
			(await dbContext
				.MediaFiles
				.Where(this.WherePredicate())
				.Include(mf => mf.MediaFileTags)
				.ThenInclude(mft => mft.Tag)
				.Include(mf => mf.ImageFile)
				.Include(mf => mf.VideoFile)
				.Include(mf => mf.Position)
				.Where(this.repositorySelector.CurrentRepositoryCondition.CurrentValue?.WherePredicate() ?? (_ => true))
				.ToArrayAsync())
				.Select(FileTypeUtility.CreateFileModelFromRecord)
				.Where(this.FilterSetter);

		return this.SortSelector.SetSortConditions(files);
	}

	/// <summary>
	/// 読み込み条件絞り込み
	/// </summary>
	/// <returns>絞り込み関数</returns>
	protected abstract Expression<Func<MediaFile, bool>> WherePredicate();

}
