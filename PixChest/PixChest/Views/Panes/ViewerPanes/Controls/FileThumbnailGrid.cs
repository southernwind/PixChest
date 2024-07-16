using System.Windows.Controls;

using PixChest.ViewModels.Files;

namespace PixChest.Views.Panes.ViewerPanes.Controls;

public class FileThumbnailGrid: Grid {
	public FileViewModel? FileViewModel {
		get;
		set;
	}
}
