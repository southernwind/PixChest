using PixChest.Composition.Bases;
using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Models.FilesFilter;

namespace PixChest.ViewModels.Panes.FilterPanes;

public class FilteringConditionViewModel : ViewModelBase {
	/// <summary>
	/// モデル
	/// </summary>
	public FilteringCondition Model {
		get;
	}

	/// <summary>
	/// 表示名
	/// </summary>
	public BindableReactiveProperty<string> DisplayName {
		get;
	}

	/// <summary>
	/// フィルター条件クリエイター
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<IFilterItemObject> FilterItems {
		get;
	}

	public FilteringConditionViewModel(FilteringCondition model) {
		this.Model = model;
		this.DisplayName = this.Model.DisplayName.ToBindableReactiveProperty(null!);

		this.FilterItems = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this.Model.FilterItemObjects);
	}
}
