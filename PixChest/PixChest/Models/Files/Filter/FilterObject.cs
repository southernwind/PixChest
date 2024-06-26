using PixChest.Models.Files.Filter.FilterItemObjects;

namespace PixChest.Models.FilesFilter; 
/// <summary>
/// フィルター設定復元用オブジェクト
/// </summary>
public class FilterObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public IReactiveProperty<string> DisplayName {
		get;
		set;
	} = new ReactivePropertySlim<string>();

	/// <summary>
	/// フィルター条件オブジェクト
	/// </summary>
	public ReactiveCollection<IFilterItemObject> FilterItemObjects {
		get;
		set;
	} = [];
}
