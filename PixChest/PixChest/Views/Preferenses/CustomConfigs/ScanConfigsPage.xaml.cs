using Microsoft.UI.Xaml.Controls;

using PixChest.Composition.Bases;
using PixChest.Utils.Enums;
using PixChest.ViewModels.Panes.ViewerPanes;
using PixChest.ViewModels.Preferenses;
using PixChest.ViewModels.Preferenses.CustomConfig;

namespace PixChest.Views.Preferenses.CustomConfig;
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
	} = [MediaType.Image, MediaType.Video];

}