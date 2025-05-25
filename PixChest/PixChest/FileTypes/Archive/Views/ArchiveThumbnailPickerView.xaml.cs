using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Views;
using PixChest.FileTypes.Archive.ViewModels;

namespace PixChest.FileTypes.Archive.Views;
public sealed partial class ArchiveThumbnailPickerView : ArchiveThumbnailPickerViewUserControl, IThumbnailPickerView {
	public ArchiveThumbnailPickerView() {
		this.InitializeComponent();
	}
}

public class ArchiveThumbnailPickerViewUserControl : UserControlBase<ArchiveThumbnailPickerViewModel> {
}
