using System.ComponentModel;

using PixChest.Composition.Bases;
using PixChest.Models.Files.Sort;
using PixChest.Models.Settings;

namespace PixChest.ViewModels.Panes.ViewerPanes;

/// <summary>
/// ソートセレクターViewModel
/// </summary>
[AddSingleton]
public class SortSelectorViewModel : ViewModelBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public SortSelectorViewModel(SortSelector model, States states) {
		this._states = states;
		this.SortConditions = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(model.SortConditions, x => new SortConditionViewModel(x));
		this.CurrentCondition.Value = this.SortConditions.FirstOrDefault(c => c.Model == model.CurrentSortCondition.Value);
		this.CurrentCondition.Subscribe(x => {
			model.CurrentSortCondition.Value = x?.Model;
		});
		this.Direction.Value = model.Direction.Value;
		this.Direction.Subscribe(x => {
			model.Direction.Value = x;
		});
	}

	private readonly States _states;
	/// <summary>
	/// カレント条件
	/// </summary>
	public BindableReactiveProperty<SortConditionViewModel?> CurrentCondition {
		get;
	} = new();

	/// <summary>
	/// ソート条件
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<SortConditionViewModel> SortConditions {
		get;
	}

	public BindableReactiveProperty<ListSortDirection> Direction {
		get;
	} = new();

	protected override void Dispose(bool disposing) {
		this._states.Save();
		base.Dispose(disposing);
	}
}
