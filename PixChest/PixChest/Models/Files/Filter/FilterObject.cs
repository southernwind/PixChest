using PixChest.Models.Files.Filter.FilterItemObjects;

namespace PixChest.Models.Files.Filter;
/// <summary>
/// フィルター設定復元用オブジェクト
/// </summary>
public class FilterObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public ReactiveProperty<string> DisplayName {
		get;
		set;
	} = new();

	/// <summary>
	/// フィルター条件オブジェクト
	/// </summary>
	public ObservableList<IFilterItemObject> FilterItemObjects {
		get;
		set;
	} = [];
}
