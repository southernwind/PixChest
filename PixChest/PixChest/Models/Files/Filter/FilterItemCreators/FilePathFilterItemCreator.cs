using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Utils.Enums;


namespace PixChest.Models.Files.Filter.FilterItemCreators;
/// <summary>
/// ファイルパスフィルタークリエイター
/// </summary>
[AddTransient]
public class FilePathFilterItemCreator : IFilterItemCreator<FilePathFilterItemObject> {
	/// <summary>
	/// フィルター条件の作成
	/// </summary>
	/// <returns>作成された条件</returns>
	public FilterItem Create(FilePathFilterItemObject filterItemObject) {
		return new FilterItem(
			x => x.FilePath.Contains(filterItemObject.Text) == (filterItemObject.SearchType == SearchTypeInclude.Include),
			x => x.FilePath.Contains(filterItemObject.Text) == (filterItemObject.SearchType == SearchTypeInclude.Include),
			true);
	}
}