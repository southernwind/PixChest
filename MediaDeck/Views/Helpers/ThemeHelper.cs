using MediaDeck.Composition.Enum;
using MediaDeck.Core.Stores.Config;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MediaDeck.Views.Helpers;

/// <summary>
/// ウィンドウのテーマ管理を補助するヘルパークラス。
/// </summary>
public static class ThemeHelper {
	/// <summary>
	/// ウィンドウに対してIConfigStoreのテーマ設定をバインドします。
	/// </summary>
	/// <param name="window">対象のウィンドウ</param>
	/// <param name="configStore">設定ストア</param>
	/// <param name="disposable">購読管理用のCompositeDisposable</param>
	public static void BindTheme(Window window, IConfigStore configStore, CompositeDisposable disposable) {
		configStore.Config.EnvironmentConfig.Theme
			.CombineLatest(configStore.Config.EnvironmentConfig.SystemBackdrop, (theme, backdrop) => new {
				Theme = theme,
				Backdrop = backdrop,
			})
			.Subscribe(x => {
				var targetTheme = x.Theme switch {
					AppTheme.Light => ElementTheme.Light,
					AppTheme.Dark => ElementTheme.Dark,
					_ => ElementTheme.Default,
				};

				if (window.Content is FrameworkElement fe) {
					fe.RequestedTheme = targetTheme;
				}

				window.SystemBackdrop = CreateSystemBackdrop(x.Backdrop);
				UpdateTitleBarColors(window, targetTheme);
			})
			.AddTo(disposable);
	}

	private static SystemBackdrop? CreateSystemBackdrop(AppSystemBackdrop backdrop) {
		return backdrop switch {
			AppSystemBackdrop.Mica => new MicaBackdrop(),
			AppSystemBackdrop.Acrylic => new DesktopAcrylicBackdrop(),
			_ => null,
		};
	}

	private static void UpdateTitleBarColors(Window window, ElementTheme theme) {
		var titleBar = window.AppWindow.TitleBar;
		var actualTheme = theme;
		if (actualTheme == ElementTheme.Default) {
			actualTheme = Application.Current.RequestedTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
		}

		if (actualTheme == ElementTheme.Dark) {
			titleBar.ButtonForegroundColor = Colors.White;
			titleBar.ButtonHoverForegroundColor = Colors.White;
			titleBar.ButtonPressedForegroundColor = Colors.White;
			titleBar.ButtonInactiveForegroundColor = Colors.Gray;

			// ダークテーマ用の控えめなホバー/プレス色
			titleBar.ButtonHoverBackgroundColor = Color.FromArgb(20, 255, 255, 255);
			titleBar.ButtonPressedBackgroundColor = Color.FromArgb(40, 255, 255, 255);
		} else {
			titleBar.ButtonForegroundColor = Colors.Black;
			titleBar.ButtonHoverForegroundColor = Colors.Black;
			titleBar.ButtonPressedForegroundColor = Colors.Black;
			titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;

			// ライトテーマ用の控えめなホバー/プレス色
			titleBar.ButtonHoverBackgroundColor = Color.FromArgb(15, 0, 0, 0);
			titleBar.ButtonPressedBackgroundColor = Color.FromArgb(30, 0, 0, 0);
		}

		// 通常時と非アクティブ時は透明
		titleBar.ButtonBackgroundColor = Colors.Transparent;
		titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
	}

	/// <summary>
	/// FrameworkElementに対してIConfigStoreのテーマ設定をバインドします。
	/// </summary>
	/// <param name="element">対象の要素</param>
	/// <param name="configStore">設定ストア</param>
	/// <param name="disposable">購読管理用のCompositeDisposable</param>
	public static void BindTheme(FrameworkElement element, IConfigStore configStore, CompositeDisposable disposable) {
		configStore.Config.EnvironmentConfig.Theme
			.Subscribe(theme => {
				element.RequestedTheme = theme switch {
					AppTheme.Light => ElementTheme.Light,
					AppTheme.Dark => ElementTheme.Dark,
					_ => ElementTheme.Default,
				};
			})
			.AddTo(disposable);
	}

	/// <summary>
	/// ContentDialogに対してIConfigStoreのテーマ設定をバインドします。
	/// </summary>
	/// <param name="dialog">対象のダイアログ</param>
	/// <param name="configStore">設定ストア</param>
	/// <param name="disposable">購読管理用のCompositeDisposable</param>
	public static void BindTheme(Microsoft.UI.Xaml.Controls.ContentDialog dialog, IConfigStore configStore, CompositeDisposable disposable) {
		configStore.Config.EnvironmentConfig.Theme
			.Subscribe(theme => {
				dialog.RequestedTheme = theme switch {
					AppTheme.Light => ElementTheme.Light,
					AppTheme.Dark => ElementTheme.Dark,
					_ => ElementTheme.Default,
				};
			})
			.AddTo(disposable);
	}
}