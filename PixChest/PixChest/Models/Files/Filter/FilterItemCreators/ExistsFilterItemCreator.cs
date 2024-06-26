using System.IO;

using PixChest.Models.FilesFilter.FilterItemObjects;
using PixChest.Models.Files.Filter.FilterItemCreators;

namespace PixChest.Models.FilesFilter.FilterItemCreators; 
/// <summary>
/// ファイル存在フィルタークリエイター
/// </summary>
public class ExistsFilterItemCreator : IFilterItemCreator<ExistsFilterItemObject> {
	/// <summary>
	/// フィルター条件の作成
	/// </summary>
	/// <returns>作成された条件</returns>
	public FilterItem Create(ExistsFilterItemObject filterItemObject) {
		return new FilterItem(
			x => File.Exists(x.FilePath) == filterItemObject.Exists,
			x => x.Exists == filterItemObject.Exists,
			false);
	}
}