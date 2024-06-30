
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;

namespace PixChest.ViewModels.Filters;

[AddTransient]
public class FilterManagerViewModel : ViewModelBase {

	private readonly States _states;

	/// <summary>
	/// カレント条件
	/// </summary>
	public BindableReactiveProperty<FilteringConditionViewModel?> CurrentCondition {
		get;
	} = new();

	/// <summary>
	/// フィルタリング条件
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<FilteringConditionViewModel> FilteringConditions {
		get;
	}

	/// <summary>
	/// フィルタリング条件追加コマンド
	/// </summary>
	public ReactiveCommand<Unit> AddFilteringConditionCommand {
		get;
	} = new();

	/// <summary>
	/// フィルタリング条件削除コマンド
	/// </summary>
	public ReactiveCommand<FilteringConditionViewModel> RemoveFilteringConditionCommand {
		get;
	} = new ReactiveCommand<FilteringConditionViewModel>();

	/// <summary>
	/// フィルター設定ウィンドウオープン
	/// </summary>
	public ReactiveCommand<Unit> OpenSetFilterWindowCommand {
		get;
	} = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterManagerViewModel(FilterDescriptionManager model, States states) {
		this._states = states;
		model.Name.Value = "set";
		this.FilteringConditions = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(model.FilteringConditions, x => new FilteringConditionViewModel(x));

		this.AddFilteringConditionCommand.Subscribe(_ => model.AddCondition());

		this.RemoveFilteringConditionCommand.Where(x => x != null).Subscribe(x => {
			model.RemoveCondition(x.Model);
		});
	}

	protected override void Dispose(bool disposing) {
		this._states.Save();
		base.Dispose(disposing);
	}
}
