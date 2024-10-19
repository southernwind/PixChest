using Microsoft.UI.Xaml;

using PixChest.ViewModels.Tools;

using Windows.Graphics;

namespace PixChest.Views.Tools;
[AddTransient]
public sealed partial class BackgroundTasksWindow : Window {
	public BackgroundTasksWindow(BackgroundTasksViewModel backgroundTasksViewModel) {
		this.InitializeComponent();
		this.ViewModel = backgroundTasksViewModel;
		this.AppWindow.Resize(new SizeInt32(400, 200));
	}

	public BackgroundTasksViewModel ViewModel {
		get;
	}
}
