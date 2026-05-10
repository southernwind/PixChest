using System.Reflection;
using MediaDeck.Common.Base;
using MediaDeck.Common.Utilities;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Composition.Interfaces.Services;

namespace MediaDeck.ViewModels.Help;

/// <summary>
/// AboutウィンドウのViewModel
/// </summary>
[Inject(InjectServiceLifetime.Transient)]
public class AboutWindowViewModel : ViewModelBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stringProvider">文字列プロバイダー</param>
	public AboutWindowViewModel(IStringProvider stringProvider, IAppPathProvider appPathProvider) {
		this.AppName = stringProvider.GetString("App_Title");
		this.Version = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();
		this.Description = stringProvider.GetString("About_Description");
		this.Copyright = stringProvider.GetString("About_Copyright");
		this.AppDataDirectoryPath = appPathProvider.BaseDirectory;

		this.OpenAppDataDirectoryCommand
			.Subscribe(_ => ShellUtility.ShellExecute(this.AppDataDirectoryPath))
			.AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// アプリケーション名
	/// </summary>
	public string AppName {
		get;
	}

	/// <summary>
	/// バージョン
	/// </summary>
	public string Version {
		get;
	}

	/// <summary>
	/// 説明
	/// </summary>
	public string Description {
		get;
	}

	/// <summary>
	/// 著作権情報
	/// </summary>
	public string Copyright {
		get;
	}

	/// <summary>
	/// アプリケーションデータフォルダのパス
	/// </summary>
	public string AppDataDirectoryPath {
		get;
	}

	/// <summary>
	/// アプリケーションデータフォルダをエクスプローラーで開くコマンド
	/// </summary>
	public ReactiveCommand OpenAppDataDirectoryCommand {
		get;
	} = new();
}