using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;
using PixChest.ViewModels.Filters;
using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Panes.FilterPanes;

/// <summary>
/// フィルターセレクターViewModel
/// </summary>
[AddTransient]
public class FilterSelectorViewModel :ViewModelBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterSelectorViewModel(FilterDescriptionManager model, States states) {
		this._states = states;
		this.FilteringConditions = model.FilteringConditions.ToReadOnlyReactiveCollection(x => new FilteringConditionViewModel(x));
		this.CurrentCondition = model.CurrentFilteringCondition.ToReactivePropertyAsSynchronized(
			x => x.Value,
			x => x == null ? null : new FilteringConditionViewModel(x),
			x => x?.Model);
	}

	private readonly States _states;
	/// <summary>
	/// カレント条件
	/// </summary>
	public IReactiveProperty<FilteringConditionViewModel?> CurrentCondition {
		get;
	}

	/// <summary>
	/// フィルタリング条件
	/// </summary>
	public ReadOnlyReactiveCollection<FilteringConditionViewModel> FilteringConditions {
		get;
	}

	protected override void Dispose(bool disposing) {
		this._states.Save();
		base.Dispose(disposing);
	}
}
