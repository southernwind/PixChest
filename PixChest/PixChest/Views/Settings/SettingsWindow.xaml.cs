using Microsoft.UI.Xaml;

using PixChest.ViewModels.Settings;

namespace PixChest.Views.Settings; 
public sealed partial class SettingsWindow : Window {
	public SettingsWindowViewModel SettingsWindowViewModel {
		get;
	}
	public SettingsWindow(SettingsWindowViewModel settingsWindowViewModel) {
		this.InitializeComponent();
		this.SettingsWindowViewModel = settingsWindowViewModel;
	}
}
