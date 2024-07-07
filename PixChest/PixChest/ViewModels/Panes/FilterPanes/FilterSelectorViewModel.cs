using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;

namespace PixChest.ViewModels.Panes.FilterPanes;

/// <summary>
/// フィルターセレクターViewModel
/// </summary>
[AddSingleton]
public class FilterSelectorViewModel :ViewModelBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterSelectorViewModel(FilterSelector model, States states) {
		this._states = states;
		this.FilteringConditions = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(model.FilteringConditions, x => new FilteringConditionViewModel(x));
		this.CurrentCondition = model.CurrentFilteringCondition.Select(x => this.FilteringConditions.FirstOrDefault(c => c.Model == x)).ToBindableReactiveProperty();
		this.ChangeFilteringConditionSelectionCommand.Subscribe(x => {
			model.CurrentFilteringCondition.Value = x?.Model;
		});
	}

	private readonly States _states;
	/// <summary>
	/// カレント条件
	/// </summary>
	public BindableReactiveProperty<FilteringConditionViewModel?> CurrentCondition {
		get;
	}

	/// <summary>
	/// フィルタリング条件
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<FilteringConditionViewModel> FilteringConditions {
		get;
	}

	public ReactiveCommand<FilteringConditionViewModel> ChangeFilteringConditionSelectionCommand {
		get;
	} = new();

	protected override void Dispose(bool disposing) {
		this._states.Save();
		base.Dispose(disposing);
	}
}
