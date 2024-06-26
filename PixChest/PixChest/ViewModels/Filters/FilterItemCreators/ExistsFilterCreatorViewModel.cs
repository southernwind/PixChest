using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// 存在フィルター作成ViewModel
/// </summary>
public class ExistsFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "存在フィルター";
		}
	}

	/// <summary>
	/// ファイルが存在するか否か
	/// </summary>
	public BindableReactiveProperty<DisplayObject<bool>> Exists {
		get;
	} = new();

	/// <summary>
	/// ファイルが存在するか否かの候補
	/// </summary>
	public IEnumerable<DisplayObject<bool>> ExistsList {
		get;
	} = [
		new DisplayObject<bool>("ファイルが存在する",true),
		new DisplayObject<bool>("ファイルが存在しない",false)
	];

	/// <summary>
	/// ファイル存在フィルター追加コマンド
	/// </summary>
	public ReactiveCommand<Unit> AddExistsFilterCommand {
		get;
	} = new();

	public ExistsFilterCreatorViewModel(FilteringCondition model) {
		this.Exists.Value = this.ExistsList.First();
		this.AddExistsFilterCommand.Subscribe(_ => {
			model.AddExistsFilter(this.Exists.Value.Value);
		});
	}
}
