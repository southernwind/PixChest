using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;
using PixChest.Views.FolderManager;
using PixChest.Views.Preferenses;

namespace PixChest.Views;
public sealed partial class NavigationMenu : NavigationMenuUserControl {
	public NavigationMenu() {
		this.InitializeComponent();
	}

	private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
		if (sender is not MenuFlyoutItem selectedItem) {
			return;
		}
		Window? window = null;
		switch (selectedItem.Tag.ToString()) {
			case "Config":
				window = Ioc.Default.GetRequiredService<ConfigWindow>();
				break;
			case "FolderManager":
				window = Ioc.Default.GetRequiredService<FolderManagerWindow>();
				break;
		}
		window?.Activate();
	}
}

public abstract class NavigationMenuUserControl : UserControlBase<NavigationMenuViewModel>;

