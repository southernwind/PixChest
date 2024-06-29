using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using PixChest.Composition.Bases;
using PixChest.Database.Tables;
using PixChest.Models.Files;
using PixChest.Models.Settings;

using Reactive.Bindings.Extensions;

namespace PixChest.Models.FilesFilter;
/// <summary>
/// フィルターマネージャー
/// </summary>
[AddSingleton]
public class FilterDescriptionManager : ModelBase {
	private readonly States _states;

	/// <summary>
	/// カレント条件
	/// </summary>
	public IReactiveProperty<FilteringCondition?> CurrentFilteringCondition {
		get;
	} = new ReactivePropertySlim<FilteringCondition?>(mode: ReactivePropertyMode.DistinctUntilChanged);

	/// <summary>
	/// フィルター条件変更通知Subject
	/// </summary>
	private readonly Subject<Unit> _onUpdateFilteringChanged = new();

	/// <summary>
	/// 設定値保存用名前
	/// </summary>
	public IReactiveProperty<string> Name {
		get;
	} = new ReactivePropertySlim<string>();

	/// <summary>
	/// フィルター条件変更通知
	/// </summary>
	public IObservable<Unit> OnFilteringConditionChanged {
		get {
			return this._onUpdateFilteringChanged.AsObservable();
		}
	}

	/// <summary>
	/// フィルター条件リスト
	/// </summary>
	public ReadOnlyReactiveCollection<FilteringCondition> FilteringConditions {
		get;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterDescriptionManager(States states) {
		this._states = states;
		IDisposable? beforeCurrent = null;
		this.CurrentFilteringCondition.CombineLatest(this.Name.Where(x => x != null), (condition, name) => (condition, name))
			.Subscribe(x => {
				this._onUpdateFilteringChanged.OnNext(Unit.Default);
				beforeCurrent?.Dispose();
				beforeCurrent = x.condition?.OnUpdateFilteringConditions
					.Subscribe(_ =>
						this._onUpdateFilteringChanged.OnNext(Unit.Default));
				states.SearchStates.CurrentFilteringCondition[x.name] = x.condition?.FilterObject;
			})
			.AddTo(this.CompositeDisposable);

		this.FilteringConditions =
			states
				.SearchStates
				.FilteringConditions
				.ToReadOnlyReactiveCollection(x => new FilteringCondition(x), ImmediateScheduler.Instance);

		// 初期カレント値読み込み
		this.Name.Where(x => x != null).Subscribe(name => {
			this.CurrentFilteringCondition.Value = this.FilteringConditions.FirstOrDefault(x => x.FilterObject == states.SearchStates.CurrentFilteringCondition[name]);
		}).AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// フィルターマネージャーで選択したフィルターを引数に渡されたクエリに適用して返却する。
	/// </summary>
	/// <param name="query">絞り込みクエリを適用するクエリ</param>
	/// <returns>フィルター適用後クエリ</returns>
	public IEnumerable<MediaFile> SetFilterConditions(IQueryable<MediaFile> query) {
		return this.CurrentFilteringCondition.Value?.SetFilterConditions(query) ?? query;
	}

	/// <summary>
	/// フィルターマネージャーで選択したフィルターを引数に渡されたシーケンスに適用して返却する。
	/// </summary>
	/// <param name="query">絞り込みを適用するシーケンス</param>
	/// <returns>フィルター適用後シーケンス</returns>
	public IEnumerable<FileModel> SetFilterConditions(IEnumerable<FileModel> files) {
		return this.CurrentFilteringCondition.Value?.SetFilterConditions(files) ?? files;
	}

	/// <summary>
	/// フィルタリング条件追加
	/// </summary>
	public void AddCondition() {
		var rfa = new FilterObject();
		this._states.SearchStates.FilteringConditions.Add(rfa);
		this.CurrentFilteringCondition.Value = new FilteringCondition(rfa);
	}

	/// <summary>
	/// フィルタリング条件削除
	/// </summary>
	/// <param name="filteringCondition">削除するフィルタリング条件</param>
	public void RemoveCondition(FilteringCondition filteringCondition) {
		this._states.SearchStates.FilteringConditions.Remove(filteringCondition.FilterObject);
	}
}
