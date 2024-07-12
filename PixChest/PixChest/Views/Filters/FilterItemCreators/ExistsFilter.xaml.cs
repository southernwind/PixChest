using PixChest.Composition.Bases;
using PixChest.ViewModels.Filters.FilterItemCreators;

namespace PixChest.Views.Filters.FilterItemCreators;

/// <summary>
/// ExistsFilter.xaml の相互作用ロジック
/// </summary>
public partial class ExistsFilter: ExistsFilterPageBase {
	public ExistsFilter() {
		this.InitializeComponent();
	}
}

public class ExistsFilterPageBase : PageBase<ExistsFilterCreatorViewModel> {
}
