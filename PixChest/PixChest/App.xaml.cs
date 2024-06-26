using Microsoft.UI.Xaml;

using PixChest.Views;

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PixChest.Composition.Bases;
using System.Windows.Controls;
using PixChest.Models.Files;
using Microsoft.Data.Sqlite;
using PixChest.Database;
using System.IO;
using R3;
using PixChest.Models.Settings;
using PixChest.Models.Preferences.CustomConfig;
using PixChest.Models.Preferences.CustomStates;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PixChest;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application {
	private Window? _window;
	private readonly string _stateFilePath;
	private readonly string _configFilePath;
	private readonly Config _config;
	private readonly States _states;

	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App() {
		var baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
		this._stateFilePath = Path.Combine(baseDirectory, "PixChest.states");
		this._configFilePath = Path.Combine(baseDirectory, "PixChest.config");
		var serviceCollection = new ServiceCollection();

		var targetTypes = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(x =>
				(
					typeof(IModelBase).IsAssignableFrom(x) ||
					typeof(IViewModelBase).IsAssignableFrom(x) ||
					typeof(UserControl).IsAssignableFrom(x)
				)
				&& !x.IsAbstract && !x.IsInterface);

		foreach (var targetType in targetTypes) {
			serviceCollection.AddTransient(targetType);
		}


		var singletonTargetTypes = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(x =>
				typeof(Window).IsAssignableFrom(x)				
				&& !x.IsAbstract && !x.IsInterface);

		foreach (var singletonTargetType in singletonTargetTypes) {
			serviceCollection.AddSingleton(singletonTargetType);
		}

		serviceCollection.AddTransient<MediaContentLibrary>();
		serviceCollection.AddSingleton<FileRegistrar>();

		// Config
		serviceCollection.AddSingleton<Config>();
		serviceCollection.AddSingleton<ScanConfig>();

		// States
		serviceCollection.AddSingleton<States>();
		serviceCollection.AddSingleton<SearchStates>();

		// DataBase
		var sb = new SqliteConnectionStringBuilder {
			DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "pix.db")
		};
		serviceCollection.AddDbContext<PixChestDbContext>(x => {
			x.UseSqlite(sb.ConnectionString);
		});
		Ioc.Default.ConfigureServices(
			serviceCollection.BuildServiceProvider()
		);
		Ioc.Default.GetRequiredService<PixChestDbContext>().Database.EnsureCreated();

		this._config = Ioc.Default.GetRequiredService<Config>();
		this._config.SetFilePath(this._configFilePath);
		this._config.Load();
		this._states = Ioc.Default.GetRequiredService<States>();
		this._states.SetFilePath(this._stateFilePath);
		this._states.Load();

		UIDispatcherScheduler.Initialize();

		this.InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched.
	/// </summary>
	/// <param name="args">Details about the launch request and process.</param>
	protected override void OnLaunched(LaunchActivatedEventArgs args) {
		this._window = Ioc.Default.GetRequiredService<MainWindow>();
		this._window.Closed += (_, _) => {
			this._states.Save();
			this._config.Save();
		};
		this._window.Activate();
	}
}
