using MediaDeck.Composition.Interfaces;
using MediaDeck.Core.Stores.State;
using MediaDeck.ViewModels.Help;
using MediaDeck.Views.Helpers;
using Microsoft.UI.Xaml;

namespace MediaDeck.Views.Help;

/// <summary>
/// ライセンス情報ウィンドウ
/// </summary>
[Inject(InjectServiceLifetime.Transient)]
public sealed partial class LicenseWindow : Window {
	private readonly CompositeDisposable _disposable = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="viewModel">ViewModel</param>
	/// <param name="stateStore">状態ストア</param>
	/// <param name="stringProvider">文字列プロバイダー</param>
	public LicenseWindow(LicenseWindowViewModel viewModel, IStateStore stateStore, IStringProvider stringProvider) {
		this.ViewModel = viewModel;
		this.InitializeComponent();

		// ウィンドウタイトルのローカライズ設定
		this.Title = stringProvider.GetString("LicenseWindow_Title");

		// テーマのバインド
		ThemeHelper.BindTheme(this, stateStore, this._disposable);

		this.AppWindow.Resize(new(600, 500));

		this.Closed += (s, e) => this._disposable.Dispose();
	}

	/// <summary>
	/// ViewModel
	/// </summary>
	public LicenseWindowViewModel ViewModel {
		get;
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(this.AppTitleBar);
	}
}