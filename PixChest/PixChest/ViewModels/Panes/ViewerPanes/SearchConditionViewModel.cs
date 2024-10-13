using PixChest.Composition.Bases;
using PixChest.Models.Files.SearchConditions;

namespace PixChest.ViewModels.Panes.ViewerPanes;
public class SearchConditionViewModel : ViewModelBase {
	public SearchConditionViewModel(ISearchCondition searchCondition) {
		this.SearchCondition = searchCondition;
		this.DisplayText = searchCondition.DisplayText;
	}

	public string DisplayText {
		get;
	}

	public ISearchCondition SearchCondition {
		get;
	}
}
