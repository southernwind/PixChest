using PixChest.Composition.Bases;
using PixChest.ViewModels.Sort;

namespace PixChest.Views.Sort;
public sealed partial class SortManagerView: SortManagerViewUserControl {
	public SortManagerView() {
		this.InitializeComponent();
	}
}

public class SortManagerViewUserControl : UserControlBase<SortManagerViewModel> {
}