using Microsoft.UI.Xaml;

using PixChest.ViewModels.Filters;

using Windows.Graphics;

namespace PixChest.Views.FolderManager;

[AddTransient]
public sealed partial class FolderManagerWindow : Window {
	public FilterManagerViewModel ViewModel {
		get;
	}
	public FolderManagerWindow(FilterManagerViewModel viewModel) {
		this.InitializeComponent();
		this.ViewModel = viewModel;
		this.AppWindow.Resize(new SizeInt32(600,400));
	}
}
