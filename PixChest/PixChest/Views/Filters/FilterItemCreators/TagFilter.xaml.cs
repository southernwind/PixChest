using PixChest.Composition.Bases;
using PixChest.ViewModels.Filters.Creators;

namespace PixChest.Views.Filters.FilterItemCreators;
/// <summary>
/// TagFilter.xaml の相互作用ロジック
/// </summary>
public partial class TagFilter: TagFilterPageBase {
	public TagFilter() {
		this.InitializeComponent();
	}
}

public class TagFilterPageBase : PageBase<TagFilterCreatorViewModel> {
}