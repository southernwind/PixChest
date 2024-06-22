using Microsoft.UI.Xaml;

using PixChest.Views;

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PixChest.Composition.Bases;
using System.Windows.Controls;
using PixChest.Models.Files;
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
		var serviceCollection = new ServiceCollection();

		var targetTypes = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(x =>
				(
					typeof(IViewModelBase).IsAssignableFrom(x) ||
					typeof(Window).IsAssignableFrom(x) ||
					typeof(UserControl).IsAssignableFrom(x)
				)
				&& !x.IsAbstract && !x.IsInterface);

		foreach (var targetType in targetTypes) {
			serviceCollection.AddTransient(targetType);
		}
		serviceCollection.AddTransient<MediaContentLibrary>();

		Ioc.Default.ConfigureServices(
			serviceCollection.BuildServiceProvider()
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
