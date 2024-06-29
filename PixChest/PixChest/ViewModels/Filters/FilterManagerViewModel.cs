using System.Reactive.Linq;

using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Filters;

[AddTransient]
public class FilterManagerViewModel : ViewModelBase {

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

	/// <summary>
	/// フィルタリング条件追加コマンド
	/// </summary>
	public ReactiveCommand AddFilteringConditionCommand {
		get;
	} = new ReactiveCommand();

	/// <summary>
	/// フィルタリング条件削除コマンド
	/// </summary>
	public ReactiveCommand<FilteringConditionViewModel> RemoveFilteringConditionCommand {
		get;
	} = new ReactiveCommand<FilteringConditionViewModel>();

	/// <summary>
	/// フィルター設定ウィンドウオープン
	/// </summary>
	public ReactiveCommand OpenSetFilterWindowCommand {
		get;
	} = new ReactiveCommand();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterManagerViewModel(FilterDescriptionManager model, States states) {
		this._states = states;
		model.Name.Value = "set";
		this.FilteringConditions = model.FilteringConditions.ToReadOnlyReactiveCollection(x => new FilteringConditionViewModel(x));
		this.CurrentCondition = model.CurrentFilteringCondition.ToReactivePropertyAsSynchronized(
			x => x.Value,
			x => x == null ? null : new FilteringConditionViewModel(x),
			x => x?.Model);

		this.AddFilteringConditionCommand.Subscribe(model.AddCondition);

		this.RemoveFilteringConditionCommand.Where(x => x != null).Subscribe(x => {
			model.RemoveCondition(x.Model);
		});
	}

	protected override void Dispose(bool disposing) {
		this._states.Save();
		base.Dispose(disposing);
	}
}
