using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.DependencyInjection;

using FFMpegCore;

using MapControl;

using MediaDeck.Composition.Constants;
using MediaDeck.Composition.Database;
using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Composition.Stores.State.Model;
using MediaDeck.Core.Models.Tools;
using MediaDeck.Core.Stores.Config;
using MediaDeck.Core.Stores.State;
using MediaDeck.Services;
using MediaDeck.ViewModels;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

using Serilog;
using Serilog.Events;

namespace MediaDeck;

public partial class App {
	private readonly IConfigStore _configStore;
	private readonly IStateStore _stateStore;
	private DispatcherQueue? _dispatcherQueue;


	/// <summary>
	///     ILoggerFactory for DI外クラスでのログ使用。
	/// </summary>
	private static ILoggerFactory LoggerFactory {
		get {
			return field ??= Ioc.Default.GetRequiredService<ILoggerFactory>();
		}
	}

	public App() {
		if (!Directory.Exists(FilePathConstants.BaseDirectory)) {
			Directory.CreateDirectory(FilePathConstants.BaseDirectory);
		}
		BuildConfigureServices();
		this._configStore = Ioc.Default.GetRequiredService<IConfigStore>();
		this._stateStore = Ioc.Default.GetRequiredService<IStateStore>();
		this.InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched.
	/// </summary>
	/// <param name="args">Details about the launch request and process.</param>
	protected override async void OnLaunched(LaunchActivatedEventArgs args) {
		this._dispatcherQueue = DispatcherQueue.GetForCurrentThread();

		// スプラッシュ画面を表示
		var splashScreen = new Views.SplashScreenWindow(this._stateStore);
		splashScreen.Activate();

		await this.InitializeAsync(splashScreen.ViewModel);

		var windowManager = Ioc.Default.GetRequiredService<WindowManager>();
		windowManager.RestoreWindows();

		var logger = LoggerFactory.CreateLogger<App>();
		AppDomain.CurrentDomain.UnhandledException += (_, e) => {
			logger.LogError(e.ExceptionObject as Exception, "UnhandledException");
		};

		// メインウィンドウが表示されたらスプラッシュ画面を閉じる
		splashScreen.Close();
	}

	/// <summary>
	/// 他プロセスからのリダイレクトアクティベーションを処理する。
	/// バックグラウンドスレッドから呼ばれるため、UIスレッドにディスパッチする。
	/// </summary>
	public void HandleRedirectedActivation(AppActivationArguments args) {
		this._dispatcherQueue?.TryEnqueue(() => {
			this._stateStore.RootState.Windows.Add(new WindowStateModel());
		});
	}

	private static void BuildConfigureServices() {
		// Serilog設定
		string[] logFields = [
			"{Timestamp:yyyy-MM-dd HH:mm:ss.fff}",
			"{Level:u4}",
			"{ThreadId:00}",
			"{Message:j}",
			"{SourceContext}",
			"{NewLine}{Exception}"
		];

		Log.Logger = new LoggerConfiguration()
			.Enrich.WithThreadId()
#if DEBUG
			.MinimumLevel.Verbose()
			.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
#else
			.MinimumLevel.Information()
#endif
			.WriteTo.Debug(outputTemplate: string.Join("｜", logFields))
			.WriteTo.File(Path.Combine(FilePathConstants.BaseDirectory, "log", ".log"),
				rollingInterval: RollingInterval.Month,
				outputTemplate: string.Join("\t", logFields))
			.CreateLogger();

		var serviceCollection = new ServiceCollection();
		serviceCollection.AddLogging(loggingBuilder => {
			loggingBuilder.AddSerilog(dispose: true);
		});

		serviceCollection.AddGeneratedServices();
		Composition.DIRegistration.AddGeneratedServices(serviceCollection);
		MediaItemTypes.DIRegistration.AddGeneratedServices(serviceCollection);
		MediaItemTypes.UI.DIRegistration.AddGeneratedServices(serviceCollection);
		ViewModels.DIRegistration.AddGeneratedServices(serviceCollection);
		Core.DIRegistration.AddGeneratedServices(serviceCollection);
		Store.DIRegistration.AddGeneratedServices(serviceCollection);

		// DataBase
		var sb = new SqliteConnectionStringBuilder { DataSource = Path.Combine(FilePathConstants.BaseDirectory, "pix.db") };
		serviceCollection.AddDbContextFactory<MediaDeckDbContext>(x => {
			x.UseSqlite(sb.ConnectionString);
		},
			ServiceLifetime.Transient);

		Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());
	}

	private async Task InitializeAsync(SplashScreenViewModel? splashViewModel = null) {

		await Task.Run(async () => {
			// OpenStreetMapのタイルサーバーにアクセスする際のUser-Agentを設定
			ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "MediaDeck/1.0 (+https://github.com/xm-i/MediaDeck)");

			// 画像メタデータ取得にSJISが必要
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			splashViewModel?.UpdateStatus("データベースを準備しています...");
			var dbPath = Path.Combine(FilePathConstants.BaseDirectory, "pix.db");
			if (!File.Exists(dbPath)) {
				{
					// 事前に接続を開いて0バイトファイルを作らないとEnsureCreatedAsyncで死ぬ
					using var __ = File.Create(dbPath);
				}
				var dbFactory = Ioc.Default.GetRequiredService<IDbContextFactory<MediaDeckDbContext>>();
				await using (var db = await dbFactory.CreateDbContextAsync()) {
					await db.Database.EnsureCreatedAsync();

					var dbVersion = db.DbVersions.AsNoTracking().FirstOrDefault(x => x.Id == 1);
					if (dbVersion == null) {
						db.DbVersions.Add(new() {
							Id = 1,
							Version = 1,
						});
						await db.SaveChangesAsync();
					}
				}
			}

			splashViewModel?.UpdateStatus("構成設定を読み込んでいます...");
			Directory.CreateDirectory(this._configStore.Config.PathConfig.TemporaryFolderPath.Value);

			GlobalFFOptions.Configure(options => {
				options.BinaryFolder = Path.Combine(this._configStore.Config.PathConfig.FFMpegFolderPath.Value);
			});

			splashViewModel?.UpdateStatus("タグ情報を初期化しています...");
			var tagsManager = Ioc.Default.GetRequiredService<ITagsManager>();
			await tagsManager.InitializeAsync();

			splashViewModel?.UpdateStatus("バックグラウンドタスクを準備しています...");
			var backgroundTasksModel = Ioc.Default.GetRequiredService<BackgroundTasksModel>();
			backgroundTasksModel.Start();

			var _ = this._stateStore.RootState.AppState.DefaultTabState.SearchState.CurrentSortCondition.Subscribe(x => Debug.WriteLine($"CurrentSortCondition {x}"));
			_ = this._stateStore.RootState.AppState.DefaultTabState.SearchState.SortDirection.Subscribe(x => Debug.WriteLine($"SortDirection {x}"));

			splashViewModel?.UpdateStatus("メディアエンジンを起動しています...");
			FlyleafLib.Engine.Start(new FlyleafLib.EngineConfig() {
#if DEBUG
				LogOutput = ":debug",
				LogLevel = FlyleafLib.LogLevel.Debug,
				FFmpegLogLevel = Flyleaf.FFmpeg.LogLevel.Warn,
#endif
				UIRefresh = false,
				FFmpegPath = this._configStore.Config.PathConfig.FFMpegFolderPath.Value,
			});
		});
	}
}