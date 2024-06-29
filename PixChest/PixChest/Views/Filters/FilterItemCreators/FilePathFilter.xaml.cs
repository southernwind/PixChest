using PixChest.Composition.Bases;
using PixChest.ViewModels.Filters.Creators;

namespace PixChest.Views.Filters.FilterItemCreators; 
/// <summary>
/// FilePathFilter.xaml の相互作用ロジック
/// </summary>
public partial class FilePathFilter: FlePathFilterPageBase {
	public FilePathFilter() {
		this.InitializeComponent();
	}
}

public class FlePathFilterPageBase : PageBase<FilePathFilterCreatorViewModel> {
}
