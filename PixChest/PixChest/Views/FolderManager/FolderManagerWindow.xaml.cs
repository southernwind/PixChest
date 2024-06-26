using Microsoft.UI.Xaml;

using PixChest.ViewModels.FolderManager;

using Windows.Graphics;

namespace PixChest.Views.FolderManager;
public sealed partial class FolderManagerWindow : Window {
	public FolderManagerViewModel ViewModel {
		get;
	}
	public FolderManagerWindow(FolderManagerViewModel viewModel) {
		this.InitializeComponent();
		this.ViewModel = viewModel;
		this.AppWindow.Resize(new SizeInt32(600,400));
	}
}
