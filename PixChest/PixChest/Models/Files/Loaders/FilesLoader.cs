using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Database.Tables;
using PixChest.Models.Files.FileTypes.Base;
using PixChest.Models.Files.FileTypes.Image;
using PixChest.Models.Files.FileTypes.Interfaces;
using PixChest.Models.Files.FileTypes.Pdf;
using PixChest.Models.Files.FileTypes.Video;
using PixChest.Models.Files.Filter;
using PixChest.Models.Files.Sort;
using PixChest.Models.Repositories;
using PixChest.Utils.Enums;

namespace PixChest.Models.Files.Loaders;

public abstract class FilesLoader(PixChestDbContext dbContext, SortSelector sortSelector,FilterSelector filterSetter, RepositorySelector repositorySelector) {
	protected FilterSelector FilterSetter = filterSetter;
	protected SortSelector SortSelector = sortSelector;
	private static IFileOperator[] _fileOperators;
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
				.Where(this.repositorySelector.CurrentRepositoryCondition.CurrentValue?.WherePredicate() ?? (_ => true))
				.ToArrayAsync())
				.Select(x => {
					return x.FilePath.GetMediaType() switch {
						MediaType.Image => new ImageFileModel(x.MediaFileId, x.FilePath) {
							ThumbnailFilePath = x.ThumbnailFileName,
							Rate = x.Rate,
							Description = x.Description,
							UsageCount = x.UsageCount,
							FileSize = x.FileSize,
							CreationTime = x.CreationTime,
							ModifiedTime = x.ModifiedTime,
							LastAccessTime = x.LastAccessTime,
							Tags = x.MediaFileTags.Select(mft => mft.Tag.TagName).ToList(),
						} as FileModel,
						MediaType.Video => new VideoFileModel(x.MediaFileId, x.FilePath) {
							ThumbnailFilePath = x.ThumbnailFileName,
							Rate = x.Rate,
							Description = x.Description,
							UsageCount = x.UsageCount,
							FileSize = x.FileSize,
							CreationTime = x.CreationTime,
							ModifiedTime = x.ModifiedTime,
							LastAccessTime = x.LastAccessTime,
							Tags = x.MediaFileTags.Select(mft => mft.Tag.TagName).ToList(),
						},
						MediaType.Pdf => new PdfFileModel(x.MediaFileId, x.FilePath) {
							ThumbnailFilePath = x.ThumbnailFileName,
							Rate = x.Rate,
							Description = x.Description,
							UsageCount = x.UsageCount,
							FileSize = x.FileSize,
							CreationTime = x.CreationTime,
							ModifiedTime = x.ModifiedTime,
							LastAccessTime = x.LastAccessTime,
							Tags = x.MediaFileTags.Select(mft => mft.Tag.TagName).ToList(),
						},
						_ => throw new Exception(),
					};
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
