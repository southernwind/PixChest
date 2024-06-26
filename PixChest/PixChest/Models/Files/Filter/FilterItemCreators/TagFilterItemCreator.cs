using PixChest.Models.Files.Filter.FilterItemCreators;
using PixChest.Models.FilesFilter.FilterItemObjects;
using PixChest.Utils.Enums;

namespace PixChest.Models.FilesFilter.FilterItemCreators; 
/// <summary>
/// タグフィルタークリエイター
/// </summary>
public class TagFilterItemCreator : IFilterItemCreator<TagFilterItemObject> {
	/// <summary>
	/// フィルター条件の作成
	/// </summary>
	/// <returns>作成された条件</returns>
	public FilterItem Create(TagFilterItemObject filterItemObject) {
		return new FilterItem(
			x => x.MediaFileTags.Select(mft => mft.Tag.TagName).Contains(filterItemObject.TagName) == (filterItemObject.SearchType == SearchTypeInclude.Include),
			x => x.Tags.Contains(filterItemObject.TagName) == (filterItemObject.SearchType == SearchTypeInclude.Include),
			false);
	}
}
