using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Core.Stores.Config;
using MediaDeck.Services;
using MediaDeck.ViewModels;
using MediaDeck.Views.Dialogs;
using MediaDeck.Views.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace MediaDeck.Views;

[Inject(InjectServiceLifetime.Transient)]
public sealed partial class MainWindow : Window {
	private readonly MainWindowViewModel _viewModel;
	private readonly CompositeDisposable _disposable = new();
	private readonly IStringProvider _stringProvider;

	public MainWindow(MainWindowViewModel viewModel, IConfigStore configStore, IStringProvider stringProvider) {
		this._viewModel = viewModel;
		this._stringProvider = stringProvider;
		this.InitializeComponent();

		this.Title = stringProvider.GetString("App_Title");

		// テーマのバインド
		ThemeHelper.BindTheme(this, configStore, this._disposable);

		// カスタムタイトルバーの設定
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(this.AppTitleBar);

		// ウィンドウ活性状態に応じたボーダー色の切り替え
		this.Activated += (s, e) => {
			if (e.WindowActivationState == WindowActivationState.Deactivated) {
				this.RootBorder.BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
			} else {
				if (Application.Current.Resources.TryGetValue("AccentFillColorDefaultBrush", out var brush)) {
					this.RootBorder.BorderBrush = (Microsoft.UI.Xaml.Media.Brush)brush;
				} else {
					this.RootBorder.BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.DeepSkyBlue);
				}
			}
		};

		this._viewModel.SelectedTab.Subscribe(tab => {
			if (tab != null && this.MainTabView.SelectedItem != tab) {
				this.MainTabView.SelectedItem = tab;
			}
		}).AddTo(this._disposable);

		this.Closed += (s, e) => this._disposable.Dispose();

		this._viewModel.WindowActivatedCommand.Execute(Unit.Default);
	}

	private void MainTabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args) {
		if (args.Item is TabViewModel tabViewModel) {
			this._viewModel.CloseTab(tabViewModel);
		}
	}

	private void MainTabView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.MainTabView.SelectedItem is TabViewModel tabViewModel) {
			if (this._viewModel.SelectedTab.Value != tabViewModel) {
				this._viewModel.SelectedTab.Value = tabViewModel;
			}
		}
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		this._viewModel.WindowActivatedCommand.Execute(Unit.Default);
	}

	private async void TabHeader_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e) {
		if (sender is FrameworkElement fe && fe.DataContext is TabViewModel tabViewModel) {
			await this.ShowRenameTabDialogAsync(tabViewModel);
			e.Handled = true;
		}
	}

	private async void TabViewItem_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e) {
		if (e.Key == Windows.System.VirtualKey.F2) {
			if (sender is FrameworkElement fe && fe.DataContext is TabViewModel tabViewModel) {
				await this.ShowRenameTabDialogAsync(tabViewModel);
				e.Handled = true;
			}
		}
	}

	private async Task ShowRenameTabDialogAsync(TabViewModel tabViewModel) {
		var dialog = Ioc.Default.GetRequiredService<TabRenameDialog>();
		dialog.XamlRoot = this.Content.XamlRoot;
		dialog.Initialize(tabViewModel.TabState.DisplayName.Value);

		var result = await dialog.ShowAsync();
		if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(dialog.ResultText)) {
			tabViewModel.TabState.DisplayName.Value = dialog.ResultText;
		}
	}

	private Brush? _originalBorderBrush;

	/// <summary>
	/// ウィンドウのボーダーをハイライト色に変更する、または元の色に戻す。
	/// ホバー時の視認性向上のために使用される。
	/// </summary>
	/// <param name="highlight">true時はハイライト色（黄色）に、false時は元の色に戻す</param>
	public void HighlightBorder(bool highlight) {
		if (highlight) {
			this._originalBorderBrush = this.RootBorder.BorderBrush;
			// ハイライト色: 黄色（でも背景の邪魔にならないように薄めの黄色）
			this.RootBorder.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 200, 0));
		} else {
			if (this._originalBorderBrush != null) {
				this.RootBorder.BorderBrush = this._originalBorderBrush;
			}
		}
	}

	private void TabContextFlyout_Opening(object sender, object e) {
		if (sender is MenuFlyout flyout) {
			var windowManager = Ioc.Default.GetRequiredService<WindowManager>();
			var currentWindowId = this._viewModel.WindowId;
			var otherWindows = windowManager.GetOtherWindows(currentWindowId);

			// メニューテキストを設定
			if (flyout.Items.FirstOrDefault() is MenuFlyoutItem openItem) {
				openItem.Text = this._stringProvider.GetString("TabViewModel_OpenInNewWindow");
			}

			// "ウィンドウへ移動" サブメニューを動的に構築
			var subItem = flyout.Items.OfType<MenuFlyoutSubItem>().FirstOrDefault();
			if (subItem != null) {
				subItem.Text = this._stringProvider.GetString("TabViewModel_MoveToWindow");
				subItem.Items.Clear();
				if (otherWindows.Count == 0) {
					var noItemText = this._stringProvider.GetString("TabViewModel_NoOtherWindows");
					var noItem = new MenuFlyoutItem { Text = noItemText, IsEnabled = false };
					subItem.Items.Add(noItem);
				} else {
					foreach (var (windowId, title) in otherWindows) {
						var item = new MenuFlyoutItem { Text = $"{title} ({windowId.ToString().Substring(0, 6)})", Tag = windowId };
						item.Click += this.MoveToExistingWindow_Click;
						item.PointerEntered += (s, e) => this.MoveWindowMenuItem_PointerEntered(s, windowId);
						item.PointerExited += (s, e) => this.MoveWindowMenuItem_PointerExited(s, windowId);
						subItem.Items.Add(item);
					}
				}
			}
		}
	}

	private void OpenInNewWindow_Click(object sender, RoutedEventArgs e) {
		if (sender is MenuFlyoutItem menuItem && menuItem.DataContext is TabViewModel tabViewModel) {
			var windowManager = Ioc.Default.GetRequiredService<WindowManager>();
			var currentWindowId = this._viewModel.WindowId;
			var tabId = tabViewModel.TabState.TabId;
			this._viewModel.DetachTab(tabViewModel);
			windowManager.MoveTabToNewWindow(tabId, currentWindowId);
		}
	}

	private void MoveToExistingWindow_Click(object sender, RoutedEventArgs e) {
		if (sender is MenuFlyoutItem menuItem && menuItem.Tag is Guid targetWindowId) {
			var tabViewModel = this.GetTabContextFromFlyout(menuItem);
			if (tabViewModel != null) {
				var windowManager = Ioc.Default.GetRequiredService<WindowManager>();
				var currentWindowId = this._viewModel.WindowId;
				var tabId = tabViewModel.TabState.TabId;
				this._viewModel.DetachTab(tabViewModel);
				windowManager.MoveTabToWindow(tabId, currentWindowId, targetWindowId);
			}
		}
	}

	private TabViewModel? GetTabContextFromFlyout(FrameworkElement element) {
		// MenuFlyoutItemのDataContextはTabViewItemのDataContext（TabViewModel）
		if (element.DataContext is TabViewModel tc) {
			return tc;
		}
		return null;
	}


	/// <summary>
	/// メニュー項目にマウスがホバーしたときに、対応するウィンドウのボーダーをハイライトする。
	/// ユーザーが移動先を視認できるようにするためのハンドラ。
	/// </summary>
	/// <param name="sender">イベント発信元のMenuFlyoutItem</param>
	/// <param name="targetWindowId">ホバーされたウィンドウID</param>
	private void MoveWindowMenuItem_PointerEntered(object sender, Guid targetWindowId) {
		var windowManager = Ioc.Default.GetRequiredService<WindowManager>();
		windowManager.HighlightWindow(targetWindowId);
	}

	/// <summary>
	/// メニュー項目からマウスがホバーを外れたときに、対応するウィンドウのボーダーハイライトを解除する。
	/// </summary>
	/// <param name="sender">イベント発信元のMenuFlyoutItem</param>
	/// <param name="targetWindowId">ホバーを外れたウィンドウID</param>
	private void MoveWindowMenuItem_PointerExited(object sender, Guid targetWindowId) {
		var windowManager = Ioc.Default.GetRequiredService<WindowManager>();
		windowManager.UnhighlightWindow(targetWindowId);
	}

}