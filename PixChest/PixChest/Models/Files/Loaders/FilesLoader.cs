using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Models.FilesFilter;

namespace PixChest.Models.Files.Loaders;

public abstract class FilesLoader(PixChestDbContext dbContext, FilterDescriptionManager filterSetter) {
	protected FilterDescriptionManager FilterSetter = filterSetter;

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
				.ToArrayAsync())
				.Select(x => new FileModel(x.FilePath) {
					ThumbnailFilePath = x.ThumbnailFileName
				})
				.Where(this.FilterSetter);
					
		return files;
	}

	/// <summary>
	/// 読み込み条件絞り込み
	/// </summary>
	/// <returns>絞り込み関数</returns>
	protected abstract Expression<Func<MediaFile, bool>> WherePredicate();

}
