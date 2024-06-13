using System;

using Microsoft.UI.Xaml;

using PixChest.Views;

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PixChest.ViewModels;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PixChest;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application {
	private Window? _window;

	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App() {
		Ioc.Default.ConfigureServices(
			new ServiceCollection()
			.AddTransient<MainWindow>()
			.AddTransient<MainWindowViewModel>()
			.BuildServiceProvider()
		);
		this.InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched.
	/// </summary>
	/// <param name="args">Details about the launch request and process.</param>
	protected override void OnLaunched(LaunchActivatedEventArgs args) {
		this._window = Ioc.Default.GetRequiredService<MainWindow>();
		this._window.Activate();
	}
}
