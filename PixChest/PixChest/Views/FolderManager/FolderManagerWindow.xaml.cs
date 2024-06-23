
using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PixChest.ViewModels.FolderManager;
using PixChest.Views.Settings;

using Windows.Graphics;
using Windows.Storage.Pickers;

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
