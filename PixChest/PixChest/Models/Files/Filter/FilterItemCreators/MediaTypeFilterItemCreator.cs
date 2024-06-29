using PixChest.Models.FilesFilter.FilterItemObjects;
using PixChest.Models.Files.Filter.FilterItemCreators;
using PixChest.Models.Settings;
using PixChest.Utils.Tools;
using PixChest.Utils.Enums;

namespace PixChest.Models.FilesFilter.FilterItemCreators;
/// <summary>
/// ファイルタイプフィルタークリエイター
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
[AddTransient]
public class MediaTypeFilterItemCreator(Config Config) : IFilterItemCreator<MediaTypeFilterItemObject> {

	/// <summary>
	/// フィルター条件の作成
	/// </summary>
	/// <returns>作成された条件</returns>
	public FilterItem Create(MediaTypeFilterItemObject filterItemObject) {
		var videoExtensions = Config.ScanConfig.TargetExtensions.Where(x => x.MediaType.Value == MediaType.Video).Select(x => x.Extension.Value);
		return new FilterItem(
			x => x.FilePath.IsVideoFile(videoExtensions.ToArray()) == filterItemObject.IsVideo,
			x => x.FilePath.IsVideoFile(videoExtensions.ToArray()) == filterItemObject.IsVideo,
			false);
	}
}
