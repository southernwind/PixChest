namespace PixChest.ViewModels.Filters.FilterItemCreators;
/// <summary>
/// フィルター作成VM
/// </summary>
public interface IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	string Title {
		get;
	}

	/// <summary>
	/// 追加コマンド
	/// </summary>
	ReactiveCommand AddFilterCommand {
		get;
	}
}
