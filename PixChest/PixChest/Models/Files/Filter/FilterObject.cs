using PixChest.Models.Files.Filter.FilterItemObjects;

namespace PixChest.Models.FilesFilter; 
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
	public Reactive.Bindings.ReactiveCollection<IFilterItemObject> FilterItemObjects {
		get;
		set;
	} = [];
}
