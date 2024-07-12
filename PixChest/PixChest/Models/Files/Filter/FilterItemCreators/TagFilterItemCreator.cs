using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Utils.Enums;

namespace PixChest.Models.Files.Filter.FilterItemCreators;
/// <summary>
/// タグフィルタークリエイター
/// </summary>
[AddTransient]
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
