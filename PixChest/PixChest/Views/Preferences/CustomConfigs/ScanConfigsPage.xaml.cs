using Microsoft.UI.Xaml.Controls;

using PixChest.Utils.Enums;
using PixChest.ViewModels.Preferences;
using PixChest.ViewModels.Preferences.CustomConfig;

namespace PixChest.Views.Preferences.CustomConfig;
public sealed partial class ScanConfigPage : Page {
	public ScanConfigPage() {
		this.InitializeComponent();
		this.DataContextChanged += (s, e) => {
			if (this.DataContext is ScanConfigPageViewModel scpvm) {
				this.ViewModel = scpvm;
			} else if (this.DataContext is ConfigWindowViewModel cwvm) {
				this.ViewModel =cwvm.ScanConfigPageViewModel;
			}
		};
	}

	public ScanConfigPageViewModel? ViewModel {
		get;
		set;
	}

	public MediaType[] MediaTypeConditions {
		get;
	} = Enum.GetValues<MediaType>();

}