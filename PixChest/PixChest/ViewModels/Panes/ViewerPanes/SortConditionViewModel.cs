using PixChest.Composition.Bases;
using PixChest.Models.Files.Sort;

namespace PixChest.ViewModels.Panes.ViewerPanes;

public class SortConditionViewModel : ViewModelBase {
	/// <summary>
	/// モデル
	/// </summary>
	public SortCondition Model {
		get;
	}

	/// <summary>
	/// 表示名
	/// </summary>
	public BindableReactiveProperty<string> DisplayName {
		get;
	}

	public SortConditionViewModel(SortCondition model) {
		this.Model = model;
		this.DisplayName = this.Model.DisplayName.ToBindableReactiveProperty(null!);
	}
}
