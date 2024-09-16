using System.Windows.Controls;

using PixChest.FileTypes.Base.ViewModels;

namespace PixChest.Views.Panes.ViewerPanes.Controls;

public class FileThumbnailGrid: Grid {
	public BaseFileViewModel? FileViewModel {
		get;
		set;
	}
}
