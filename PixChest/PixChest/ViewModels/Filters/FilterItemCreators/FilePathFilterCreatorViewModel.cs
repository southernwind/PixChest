using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// ファイルパスフィルター作成ViewModel
/// </summary>
public class FilePathFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "ファイルパスフィルター";
		}

	}

	/// <summary>
	/// タグ名
	/// </summary>
	public BindableReactiveProperty<string> FilePath {
		get;
	} = new();

	/// <summary>
	/// 検索条件として指定のタグを含むものを検索するか、含まないものを検索するかを選択する。
	/// </summary>
	public BindableReactiveProperty<DisplayObject<SearchTypeInclude>> SearchType {
		get;
	} = new();

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
	/// ファイルパスフィルター追加コマンド
	/// </summary>
	public ReactiveCommand<Unit> AddFilePathFilterCommand {
		get;
	}

	public FilePathFilterCreatorViewModel(FilteringCondition model) {
		this.SearchType.Value = this.SearchTypeList.First();
		this.AddFilePathFilterCommand = this.FilePath.Select(x => !string.IsNullOrEmpty(x)).ToReactiveCommand();
		this.AddFilePathFilterCommand.Subscribe(_ => model.AddFilePathFilter(this.FilePath.Value, this.SearchType.Value.Value)).AddTo(this.CompositeDisposable);
	}
}
