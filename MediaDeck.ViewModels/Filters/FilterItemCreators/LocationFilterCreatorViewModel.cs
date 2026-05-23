using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Core.Models.Files.Filter.FilterItemObjects;
using MediaDeck.Core.Primitives;

namespace MediaDeck.ViewModels.Filters.FilterItemCreators;

/// <summary>
/// 座標フィルター作成ViewModel
/// </summary>
public class LocationFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	private readonly IStringProvider _stringProvider;

	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return this._stringProvider.GetString("FilterCreator_Location_Title");
		}
	}

	/// <summary>
	/// フィルター追加コマンド
	/// </summary>
	public ReactiveCommand AddFilterCommand {
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
	}

	public LocationFilterCreatorViewModel(ReactiveProperty<FilteringConditionEditorViewModel?> target, IStringProvider stringProvider) {
		this._stringProvider = stringProvider;
		this.HasLocationList = [
			new DisplayObject<bool>(this._stringProvider.GetString("FilterCreator_Location_Contains_True"), true),
			new DisplayObject<bool>(this._stringProvider.GetString("FilterCreator_Location_Contains_False"), false)
		];
		this.HasLocation.Value = this.HasLocationList.First();
		this.AddFilterCommand.Subscribe(vm => {
			var filter = new LocationFilterItemObject {
				Contains = this.HasLocation.Value.Value
			};
			target.Value?.AddFilter(filter);
		}).AddTo(this.CompositeDisposable);
	}
}