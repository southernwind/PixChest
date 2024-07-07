using System.Collections.Generic;
using System.Reactive.Linq;

using PixChest.Composition.Bases;
using PixChest.Database.Tables;
using PixChest.Models.Files;
using PixChest.Models.Settings;

namespace PixChest.Models.FilesFilter;
/// <summary>
/// フィルターマネージャー
/// </summary>
[AddSingleton]
public class FilterSelector : ModelBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public FilterSelector(States states) {
		this._states = states;

		this.FilteringConditions =
			Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(
				states.SearchStates.FilteringConditions,
				x => new FilteringCondition(x));

		this.CurrentFilteringCondition.Value = this.FilteringConditions.FirstOrDefault(x => x.FilterObject == states.SearchStates.CurrentFilteringCondition.Value);

		IDisposable? beforeCurrent = null;
		this.CurrentFilteringCondition
			.Subscribe(x => {
				this._onUpdateFilteringChanged.OnNext(Unit.Default);
				beforeCurrent?.Dispose();
				beforeCurrent = x?.OnUpdateFilteringConditions
					.Subscribe(_ =>
						this._onUpdateFilteringChanged.OnNext(Unit.Default));
				states.SearchStates.CurrentFilteringCondition.Value = x?.FilterObject;
			})
			.AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// フィルター条件変更通知Subject
	/// </summary>
	private readonly Subject<Unit> _onUpdateFilteringChanged = new Subject<Unit>();

	/// <summary>
	/// フィルター条件変更通知
	/// </summary>
	public Observable<Unit> OnFilteringConditionChanged {
		get {
			return this._onUpdateFilteringChanged.AsObservable();
		}
	}

	private readonly States _states;
	/// <summary>
	/// カレント条件
	/// </summary>
	public ReactiveProperty<FilteringCondition?> CurrentFilteringCondition {
		get;
	} = new();

	/// <summary>
	/// フィルター条件リスト
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<FilteringCondition> FilteringConditions {
		get;
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
}
