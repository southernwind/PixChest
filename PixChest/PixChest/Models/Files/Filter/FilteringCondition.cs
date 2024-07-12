using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Models.Files.Filter.FilterItemObjects;
using System.Reactive.Concurrency;
using PixChest.Database.Tables;

namespace PixChest.Models.Files.Filter;
/// <summary>
/// フィルタリング条件
/// </summary>
/// <remarks>
/// Add***Filterメソッドでフィルタークリエイターを<see cref="FilterItemObjects"/>に追加し、
/// <see cref="RemoveFilter(IFilterItemObject)"/>メソッドで削除する。
/// </remarks>
public class FilteringCondition : ModelBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="filterObject">復元用フィルターオブジェクト</param>
	public FilteringCondition(FilterObject filterObject) {
		this.FilterObject = filterObject;
		this.DisplayName = filterObject.DisplayName.ToReadOnlyReactiveProperty();
		this.FilterItemObjects =
			Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this.FilterObject.FilterItemObjects);

		this._filterItems = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this.FilterItemObjects, FilterItemFactory.Create, Scheduler.Immediate);

		Reactive.Bindings.Extensions.INotifyCollectionChangedExtensions.CollectionChangedAsObservable(this._filterItems).Subscribe(x => {
			this._onUpdateFilteringConditions.OnNext(Unit.Default);
		}).AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// 表示名
	/// </summary>
	public ReadOnlyReactiveProperty<string> DisplayName {
		get;
	}

	/// <summary>
	/// フィルター条件
	/// </summary>
	private readonly Reactive.Bindings.ReadOnlyReactiveCollection<FilterItem> _filterItems;

	/// <summary>
	/// フィルター条件クリエイター
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<IFilterItemObject> FilterItemObjects {
		get;
	}

	/// <summary>
	/// フィルター条件変更通知Subject
	/// </summary>
	private readonly Subject<Unit> _onUpdateFilteringConditions = new Subject<Unit>();

	/// <summary>
	/// フィルター条件変更通知
	/// </summary>
	public Observable<Unit> OnUpdateFilteringConditions {
		get {
			return this._onUpdateFilteringConditions.AsObservable();
		}
	}
	/// <summary>
	/// フィルター保存用オブジェクト
	/// </summary>
	public FilterObject FilterObject {
		get;
	}

	/// <summary>
	/// フィルター条件に合致しているか判定する
	/// </summary>
	/// <param name="query">絞り込みクエリを適用するクエリ</param>
	/// <returns>結果</returns>
	public IEnumerable<MediaFile> SetFilterConditions(IQueryable<MediaFile> query) {
		foreach (var filter in this._filterItems.Where(x => x.IncludeSql)) {
			query = query.Where(filter.Condition);
		}

		var mfs = query.AsEnumerable();
		foreach (var filter in this._filterItems.Where(x => !x.IncludeSql)) {
			mfs = mfs.Where(filter.Condition.Compile());
		}
		return mfs;
	}

	/// <summary>
	/// フィルター条件に合致しているか判定する
	/// </summary>
	/// <param name="files">絞り込みを適用するモデルシーケンス</param>
	/// <returns>絞り込み後シーケンス</returns>
	public IEnumerable<FileModel> SetFilterConditions(IEnumerable<FileModel> files) {
		foreach (var filter in this._filterItems) {
			files = files.Where(filter.ConditionForModel);
		}
		return files;
	}
}
