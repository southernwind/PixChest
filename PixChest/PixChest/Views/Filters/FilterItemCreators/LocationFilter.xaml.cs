using PixChest.Composition.Bases;
using PixChest.ViewModels.Filters.Creators;

namespace PixChest.Views.Filters.FilterItemCreators; 
/// <summary>
/// LocationFilter.xaml の相互作用ロジック
/// </summary>
public partial class LocationFilter: LocationFilterPageBase {
	public LocationFilter() {
		this.InitializeComponent();
	}
}

public class LocationFilterPageBase : PageBase<LocationFilterCreatorViewModel> {
}