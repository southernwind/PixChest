using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Models.FilesFilter;

namespace PixChest.Models.Files.Filter.FilterItemCreators;

/// <summary>
/// フィルタークリエイターインターフェイス
/// </summary>
public interface IFilterItemCreator<in T> where T : IFilterItemObject {

	/// <summary>
	/// フィルター条件の作成
	/// </summary>
	/// <returns>作成された条件</returns>
	FilterItem Create(T filterItemObject);
}