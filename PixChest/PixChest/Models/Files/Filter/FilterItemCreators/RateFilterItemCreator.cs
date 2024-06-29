using PixChest.Models.FilesFilter.FilterItemObjects;
using PixChest.Models.Files.Filter.FilterItemCreators;
using PixChest.Utils.Enums;

namespace PixChest.Models.FilesFilter.FilterItemCreators;
/// <summary>
/// 評価フィルタークリエイター
/// </summary>
[AddTransient]
public class RateFilterItemCreator : IFilterItemCreator<RateFilterItemObject> {
	/// <summary>
	/// フィルター条件の作成
	/// </summary>
	/// <returns>作成された条件</returns>
	public FilterItem Create(RateFilterItemObject filterItemObject) {
		var op = SearchTypeConverters.SearchTypeToFunc<int>(filterItemObject.SearchType);
		return new FilterItem(
			x => op(x.Rate, filterItemObject.Rate),
			x => op(x.Rate.Value, filterItemObject.Rate),
			false);
	}
}
