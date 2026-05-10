using System.Reflection;
using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces;

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
	public AboutWindowViewModel(IStringProvider stringProvider) {
		this.AppName = stringProvider.GetString("App_Title");
		this.Version = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();
		this.Description = stringProvider.GetString("About_Description");
		this.Copyright = stringProvider.GetString("About_Copyright");
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
}