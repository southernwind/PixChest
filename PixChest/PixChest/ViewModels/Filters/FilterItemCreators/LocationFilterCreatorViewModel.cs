using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// 座標フィルター作成ViewModel
/// </summary>
public class LocationFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "座標フィルター";
		}
	}

	/// <summary>
	/// メディアタイプフィルター追加コマンド
	/// </summary>
	public ReactiveCommand<Unit> AddLocationFilterCommand {
		get;
	} = new();

	/// <summary>
	/// 座標情報を持っているか否か
	/// </summary>
	public BindableReactiveProperty<DisplayObject<bool>> HasLocation {
		get;
	} = new();

	/// <summary>
	/// 座標情報を持っているか否かの候補
	/// </summary>
	public IEnumerable<DisplayObject<bool>> HasLocationList {
		get;
	} = [
		new DisplayObject<bool>("座標情報を含む",true),
		new DisplayObject<bool>("座標情報を含まない",false)
	];

	public LocationFilterCreatorViewModel(FilteringCondition model) {
		this.HasLocation.Value = this.HasLocationList.First();
		this.AddLocationFilterCommand.Subscribe(_ => {
			model.AddLocationFilter(this.HasLocation.Value.Value);
		});
	}
}
