using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PixChest.ViewModels.Preferenses;
using PixChest.Views.Preferenses.CustomConfig;

namespace PixChest.Views.Preferenses; 
public sealed partial class ConfigWindow : Window {
	public ConfigWindowViewModel ViewModel {
		get;
	}
	public ConfigWindow(ConfigWindowViewModel ConfigWindowViewModel) {
		this.InitializeComponent();
		this.ViewModel = ConfigWindowViewModel;
	}

	private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
		if (args.SelectedItem is not NavigationViewItem selectedItem) {
			return;
		}

		switch(selectedItem.Tag) {
			case "ScanConfig":
				this.ContentFrame.Navigate(typeof(ScanConfigPage));
				break;
		}
	}
}
