using Microsoft.UI.Xaml;

using PixChest.Views;

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Data.Sqlite;
using PixChest.Database;
using System.IO;
using R3;
using PixChest.Models.Preferences;
using FFMpegCore;
namespace PixChest;

public partial class App : Application {
	private Window? _window;
	private readonly string _stateFilePath;
	private readonly string _configFilePath;
	private readonly Config _config;
	private readonly States _states;

	public App() {
		var baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;
		this._stateFilePath = Path.Combine(baseDirectory, "PixChest.states");
		this._configFilePath = Path.Combine(baseDirectory, "PixChest.config");
		var serviceCollection = new ServiceCollection();

		var targetTypes = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(x =>
				x.GetCustomAttributes<AddTransientAttribute>(inherit: true).Any());

		foreach (var targetType in targetTypes) {
			serviceCollection.AddTransient(targetType);
		}

		var singletonTargetTypes = Assembly
			.GetExecutingAssembly()
			.GetTypes()
			.Where(x =>
				x.GetCustomAttributes<AddSingletonAttribute>(inherit: true).Any());

		foreach (var singletonTargetType in singletonTargetTypes) {
			serviceCollection.AddSingleton(singletonTargetType);
		}

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
		Directory.CreateDirectory(this._config.PathConfig.TemporaryFolderPath.Value);

		Reactive.Bindings.UIDispatcherScheduler.Initialize();

		GlobalFFOptions.Configure(options => {
			options.BinaryFolder = Path.Combine(this._config.PathConfig.FFMpegFolderPath.Value);
		});
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
