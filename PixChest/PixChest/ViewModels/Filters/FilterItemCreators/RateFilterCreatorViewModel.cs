using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;
using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// 評価フィルター作成ViewModel
/// </summary>
public class RateFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "評価フィルター";
		}
	}

	/// <summary>
	/// 評価 チェック用テキスト
	/// </summary>
	[Range(0, 5)]
	public ReactiveProperty<string?> RateText {
		get;
	}

	/// <summary>
	/// 検索条件として指定のタグを含むものを検索するか、含まないものを検索するかを選択する。
	/// </summary>
	public IReactiveProperty<DisplayObject<SearchTypeComparison>> SearchType {
		get;
	} = new ReactivePropertySlim<DisplayObject<SearchTypeComparison>>();

	/// <summary>
	/// 含む/含まないの選択候補
	/// </summary>
	public IEnumerable<DisplayObject<SearchTypeComparison>> SearchTypeList {
		get;
	} = [
		new DisplayObject<SearchTypeComparison>("を超える",SearchTypeComparison.GreaterThan),
		new DisplayObject<SearchTypeComparison>("以上",SearchTypeComparison.GreaterThanOrEqual),
		new DisplayObject<SearchTypeComparison>("と等しい",SearchTypeComparison.Equal),
		new DisplayObject<SearchTypeComparison>("以下",SearchTypeComparison.LessThanOrEqual),
		new DisplayObject<SearchTypeComparison>("未満",SearchTypeComparison.LessThan)
	];

	/// <summary>
	/// 評価フィルター追加コマンド
	/// </summary>
	public ReactiveCommand AddRateFilterCommand {
		get;
	}

	public RateFilterCreatorViewModel(FilteringCondition model) {
		this.RateText = new ReactiveProperty<string?>().SetValidateAttribute(() => this.RateText);
		this.SearchType.Value = this.SearchTypeList.First(x => x.Value == SearchTypeComparison.GreaterThanOrEqual);
		this.AddRateFilterCommand = new[] {
				this.RateText.Select(string.IsNullOrEmpty),
				this.RateText.ObserveHasErrors
			}.CombineLatestValuesAreAllFalse()
			.ToReactiveCommand()
			.AddTo(this.CompositeDisposable);
		this.AddRateFilterCommand
			.Subscribe(_ => {
				if (int.TryParse(this.RateText.Value, out var r)) {
					model.AddRateFilter(r, this.SearchType.Value.Value);
				}

				this.RateText.Value = null;
			})
			.AddTo(this.CompositeDisposable);

	}
}
