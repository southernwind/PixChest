using System.Collections.Generic;
using PixChest.Models.FilesFilter.FilterItemObjects;
using PixChest.Composition.Bases;
using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Database.Tables;
using PixChest.Models.Files;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;
using Reactive.Bindings.Extensions;

namespace PixChest.Models.FilesFilter;
/// <summary>
/// フィルタリング条件
/// </summary>
/// <remarks>
/// Add***Filterメソッドでフィルタークリエイターを<see cref="FilterItemObjects"/>に追加し、
/// <see cref="RemoveFilter(IFilterItemObject)"/>メソッドで削除する。
/// 追加されたフィルタークリエイターはフィルターを生成し、内部に保持する。
/// </remarks>
public class FilteringCondition : ModelBase {
	/// <summary>
	/// 表示名
	/// </summary>
	public ReactiveProperty<string> DisplayName {
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
	/// コンストラクタ
	/// </summary>
	/// <param name="filterObject">復元用フィルターオブジェクト</param>
	public FilteringCondition(FilterObject filterObject) {
		this.FilterObject = filterObject;
		this.DisplayName = filterObject.DisplayName.ToBindableReactiveProperty(null!);
		this.FilterItemObjects =
			Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this.FilterObject.FilterItemObjects);

		this._filterItems =
			Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this.FilterItemObjects,FilterItemFactory.Create, System.Reactive.Concurrency.Scheduler.Immediate);

		this._filterItems.CollectionChangedAsObservable().Subscribe(x => {
			this._onUpdateFilteringConditions.OnNext(Unit.Default);
		}).AddTo(this.CompositeDisposable);

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

	/// <summary>
	/// タグフィルター追加
	/// </summary>
	/// <param name="tagName">タグ名</param>
	/// <param name="searchType">検索タイプ</param>
	public void AddTagFilter(string tagName, SearchTypeInclude searchType) {
		this.FilterObject.FilterItemObjects.Add(
			new TagFilterItemObject(tagName, searchType)
		);
	}

	/// <summary>
	/// ファイルパスフィルター追加
	/// </summary>
	/// <param name="text">ファイルパスに含まれる文字列</param>
	/// <param name="searchType">検索タイプ</param>
	public void AddFilePathFilter(string text, SearchTypeInclude searchType) {
		this.FilterObject.FilterItemObjects.Add(
			new FilePathFilterItemObject(text, searchType)
		);
	}

	/// <summary>
	/// 評価フィルター追加
	/// </summary>
	/// <param name="rate">評価</param>
	/// <param name="searchType">検索タイプ</param>
	public void AddRateFilter(int rate, SearchTypeComparison searchType) {
		this.FilterObject.FilterItemObjects.Add(
			new RateFilterItemObject(rate, searchType)
		);
	}

	/// <summary>
	/// 解像度フィルター追加
	/// </summary>
	/// <param name="width">幅</param>
	/// <param name="height">高さ</param>
	/// <param name="searchType">検索タイプ</param>
	public void AddResolutionFilter(int? width, int? height, SearchTypeComparison searchType) {
		IFilterItemObject filterItemObject;
		if (width is { } w && height is { } h) {
			filterItemObject = new ResolutionFilterItemObject(new ComparableSize(w, h), searchType);
		} else {
			filterItemObject = new ResolutionFilterItemObject(width, height, searchType);
		}
		this.FilterObject.FilterItemObjects.Add(filterItemObject);
	}

	/// <summary>
	/// メディアタイプフィルター追加
	/// </summary>
	/// <param name="isVideo">動画か否か</param>
	public void AddMediaTypeFilter(bool isVideo) {
		this.FilterObject.FilterItemObjects.Add(
			new MediaTypeFilterItemObject(isVideo)
		);
	}

	/// <summary>
	/// 座標フィルター追加
	/// </summary>
	/// <param name="hasLocation">座標情報を含むか否か</param>
	public void AddLocationFilter(bool hasLocation) {
		this.FilterObject.FilterItemObjects.Add(
			new LocationFilterItemObject(hasLocation)
		);
	}

	/// <summary>
	///ファイル存在フィルター追加
	/// </summary>
	/// <param name="exists">ファイルが存在するか否か</param>
	public void AddExistsFilter(bool exists) {
		this.FilterObject.FilterItemObjects.Add(
			new ExistsFilterItemObject(exists)
		);
	}

	/// <summary>
	/// フィルター削除
	/// </summary>
	/// <param name="filterItemObject">削除対象フィルタークリエイター</param>
	public void RemoveFilter(IFilterItemObject filterItemObject) {
		this.FilterObject.FilterItemObjects.Remove(filterItemObject);
	}
}
