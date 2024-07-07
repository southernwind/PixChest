
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;

namespace PixChest.ViewModels.Filters;

[AddTransient]
public class FilterManagerViewModel : ViewModelBase {

	private readonly States _states;
	private readonly FilterManager _filterManager;

	/// <summary>
	/// カレント条件
	/// </summary>
	public BindableReactiveProperty<FilteringConditionEditorViewModel?> CurrentCondition {
		get;
	} = new();

	/// <summary>
	/// フィルタリング条件
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<FilteringConditionEditorViewModel> FilteringConditions {
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
	public ReactiveCommand<FilteringConditionEditorViewModel> RemoveFilteringConditionCommand {
		get;
	} = new ReactiveCommand<FilteringConditionEditorViewModel>();

	/// <summary>
	/// 保存コマンド
	/// </summary>
	public ReactiveCommand<Unit> SaveCommand {
		get;
	} = new();

	/// <summary>
	/// 読み込みコマンド
	/// </summary>
	public ReactiveCommand<Unit> LoadCommand {
		get;
	} = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterManagerViewModel(FilterManager filterManager, States states) {
		this._states = states;
		this._filterManager = filterManager;

		this.AddFilteringConditionCommand.Subscribe(_ => {
			this.Add();
		});

		this.RemoveFilteringConditionCommand.Where(x => x != null).Subscribe(this.Remove);

		this.SaveCommand.Subscribe(_ => {
			this.Save();
		}).AddTo(this.CompositeDisposable);

		this.LoadCommand.Subscribe(_ => {
			this.Load();
		}).AddTo(this.CompositeDisposable);

		this.FilteringConditions = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(filterManager.FilteringConditions,x => new FilteringConditionEditorViewModel(x));
	}

	/// <summary>
	/// 追加
	/// </summary>
	public void Add() {
		this._filterManager.AddCondition();
	}

	/// <summary>
	/// 削除
	/// </summary>
	public void Remove(FilteringConditionEditorViewModel filteringConditionViewModel) {
		this._filterManager.RemoveCondition(filteringConditionViewModel.Model);
	}

	/// <summary>
	/// 読み込み
	/// </summary>
	public void Load() {
		this._filterManager.Load();
	}

	/// <summary>
	/// 保存
	/// </summary>
	public void Save() {
		this._filterManager.Save();
	}
}
