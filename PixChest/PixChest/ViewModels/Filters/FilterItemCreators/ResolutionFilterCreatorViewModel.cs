using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;

using PixChest.Composition.Bases;
using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Models.FilesFilter;
using PixChest.Models.FilesFilter.FilterItemObjects;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// 解像度フィルター作成ViewModel
/// </summary>
public class ResolutionFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "解像度フィルター";
		}
	}

	/// <summary>
	/// フィルター追加コマンド
	/// </summary>
	public ReactiveCommand<Unit> AddFilterCommand {
		get;
	}

	/// <summary>
	/// 解像度幅
	/// </summary>
	[Range(0d, int.MaxValue)]
	public BindableReactiveProperty<string?> ResolutionWidthText {
		get;
	}

	/// <summary>
	/// 解像度高さ
	/// </summary>
	[Range(0d, int.MaxValue)]
	public BindableReactiveProperty<string?> ResolutionHeightText {
		get;
	}

	/// <summary>
	/// 検索タイプを選択
	/// </summary>
	public BindableReactiveProperty<DisplayObject<SearchTypeComparison>> SearchType {
		get;
	} = new();

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

	public ResolutionFilterCreatorViewModel(ReactiveProperty<FilteringConditionEditorViewModel?> target) {
		this.ResolutionWidthText = new BindableReactiveProperty<string?>().EnableValidation(() => this.ResolutionWidthText);
		this.ResolutionHeightText = new BindableReactiveProperty<string?>().EnableValidation(() => this.ResolutionHeightText);
		this.SearchType.Value = this.SearchTypeList.First(x => x.Value == SearchTypeComparison.GreaterThanOrEqual);

		this.AddFilterCommand =
			this.ResolutionWidthText.Select(string.IsNullOrEmpty).CombineLatest(
				this.ResolutionHeightText.Select(string.IsNullOrEmpty),
				this.ResolutionWidthText.ErrorsChangedAsObservable().Select(_ => this.ResolutionWidthText.HasErrors).ToObservable(),
				this.ResolutionHeightText.ErrorsChangedAsObservable().Select(_ => this.ResolutionHeightText.HasErrors).ToObservable(),
				(x, x2, x3, x4) => !x && !x2 && !x3 && !x4
				).ToReactiveCommand<Unit>();

		this.AddFilterCommand
			.Subscribe(vm => {
				int? width = null;
				int? height = null;
				if (int.TryParse(this.ResolutionWidthText.Value, out var w)) {
					width = w;
				}
				if (int.TryParse(this.ResolutionHeightText.Value, out var h)) {
					height = h;
				}

				IFilterItemObject filterItemObject;
				if (width is { } w2 && height is { } h2) {
					filterItemObject = new ResolutionFilterItemObject(new ComparableSize(w2, h2), this.SearchType.Value.Value);
				} else {
					filterItemObject = new ResolutionFilterItemObject(width, height, this.SearchType.Value.Value);
				}
				target.Value?.AddFilter(filterItemObject);
			})
			.AddTo(this.CompositeDisposable);
	}
}
