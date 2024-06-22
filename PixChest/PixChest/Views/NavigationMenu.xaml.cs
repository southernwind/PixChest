using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;

using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;
using PixChest.Views.Settings;

namespace PixChest.Views;
public sealed partial class NavigationMenu : NavigationMenuUserControl {
	public NavigationMenu() {
		this.InitializeComponent();
	}

	private void MenuFlyoutItem_Click(object _, RoutedEventArgs __) {
		var settingsWindow = Ioc.Default.GetRequiredService<SettingsWindow>();
		settingsWindow.Activate();
	}
}

public abstract class NavigationMenuUserControl : UserControlBase<NavigationMenuViewModel>;

