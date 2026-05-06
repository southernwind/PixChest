using MediaDeck.Common.Base;
using R3;

namespace MediaDeck.ViewModels;

/// <summary>
/// スプラッシュ画面のViewModel。
/// </summary>
public class SplashScreenViewModel : ViewModelBase {
	/// <summary>
	/// 現在の状態メッセージ。
	/// </summary>
	public BindableReactiveProperty<string> StatusMessage { get; }

	/// <summary>
	/// アプリケーションのバージョン。
	/// </summary>
	public string AppVersion { get; }

	private readonly ReactiveProperty<string> _statusMessage = new("起動しています...");

	public SplashScreenViewModel() {
		this.StatusMessage = this._statusMessage.ObserveOnCurrentSynchronizationContext().ToBindableReactiveProperty<string>();
		this.StatusMessage.AddTo(this.CompositeDisposable);
		this._statusMessage.AddTo(this.CompositeDisposable);

		// バージョン情報の取得
		var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
		this.AppVersion = version != null ? $"v{version.Major}.{version.Minor}.{version.Build}" : "v1.0.0";
	}

	/// <summary>
	/// ステータスメッセージを更新します。
	/// </summary>
	/// <param name="message">メッセージ</param>
	public void UpdateStatus(string message) {
		this._statusMessage.Value = message;
	}
}
