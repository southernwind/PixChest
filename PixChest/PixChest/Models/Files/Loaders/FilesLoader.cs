using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Models.Files.Filter;
using PixChest.Models.Files.Sort;
using PixChest.Models.Repositories;

namespace PixChest.Models.Files.Loaders;

public abstract class FilesLoader(PixChestDbContext dbContext, SortSelector sortSelector,FilterSelector filterSetter, RepositorySelector repositorySelector) {
	protected FilterSelector FilterSetter = filterSetter;
	protected SortSelector SortSelector = sortSelector;
	private readonly RepositorySelector repositorySelector = repositorySelector;

	public async Task<IEnumerable<FileModel>> Load() {
		var files =
			(await dbContext
				.MediaFiles
				.Where(this.WherePredicate())
				.Include(mf => mf.MediaFileTags)
				.ThenInclude(mft => mft.Tag)
				.Include(mf => mf.ImageFile)
				.Include(mf => mf.VideoFile)
				.Include(mf => mf.Position)
				.Where(x => x.ThumbnailFileName != null)
				.Where(this.repositorySelector.CurrentRepositoryCondition.CurrentValue?.WherePredicate() ?? (_ => true))
				.ToArrayAsync())
				.Select(x => {
					var file = new FileModel(x.MediaFileId, x.FilePath) {
						ThumbnailFilePath = x.ThumbnailFileName,
						FileSize = x.FileSize,
						CreationTime = x.CreationTime,
						ModifiedTime = x.ModifiedTime,
						LastAccessTime = x.LastAccessTime,
						Tags = x.MediaFileTags.Select(mft => mft.Tag.TagName).ToList(),
					};
					return file;
				})
				.Where(this.FilterSetter);

		return this.SortSelector.SetSortConditions(files);
	}

	/// <summary>
	/// 読み込み条件絞り込み
	/// </summary>
	/// <returns>絞り込み関数</returns>
	protected abstract Expression<Func<MediaFile, bool>> WherePredicate();

}
