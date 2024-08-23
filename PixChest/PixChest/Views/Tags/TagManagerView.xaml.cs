using PixChest.Composition.Bases;
using PixChest.ViewModels.Tags;

namespace PixChest.Views.Tags;
public sealed partial class TagManagerView : TagManagerViewUserControl {
	public TagManagerView() {
		this.InitializeComponent();
	}
}

public class TagManagerViewUserControl : UserControlBase<TagManagerViewModel> {
}