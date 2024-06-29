using System.Collections.Generic;
using System.Reactive.Linq;
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;
using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// タグフィルター作成ViewModel
/// </summary>
public class TagFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "タグフィルター";
		}
	}

	/// <summary>
	/// タグ名
	/// </summary>
	public IReactiveProperty<string> TagName {
		get;
	} = new ReactivePropertySlim<string>();

	/// <summary>
	/// 検索条件として指定のタグを含むものを検索するか、含まないものを検索するかを選択する。
	/// </summary>
	public IReactiveProperty<DisplayObject<SearchTypeInclude>> SearchType {
		get;
	} = new ReactivePropertySlim<DisplayObject<SearchTypeInclude>>();

	/// <summary>
	/// 含む/含まないの選択候補
	/// </summary>
	public IEnumerable<DisplayObject<SearchTypeInclude>> SearchTypeList {
		get;
	} = [
		new DisplayObject<SearchTypeInclude>("含む",SearchTypeInclude.Include),
		new DisplayObject<SearchTypeInclude>("含まない",SearchTypeInclude.Exclude)
	];

	/// <summary>
	/// タグフィルター追加コマンド
	/// </summary>
	public ReactiveCommand AddTagFilterCommand {
		get;
	}

	public TagFilterCreatorViewModel(FilteringCondition model) {
		this.SearchType.Value = this.SearchTypeList.First();
		this.AddTagFilterCommand = this.TagName.Select(x => !string.IsNullOrEmpty(x)).ToReactiveCommand().AddTo(this.CompositeDisposable);
		this.AddTagFilterCommand.Subscribe(_ => model.AddTagFilter(this.TagName.Value, this.SearchType.Value.Value)).AddTo(this.CompositeDisposable);
	}
}
