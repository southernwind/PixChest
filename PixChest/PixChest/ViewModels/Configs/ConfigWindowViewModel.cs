using PixChest.Composition.Bases;
using PixChest.Models.Settings;
using PixChest.ViewModels.Preferences.CustomConfig;

namespace PixChest.ViewModels.Preferences;

[AddTransient]
public class ConfigWindowViewModel : ViewModelBase {
	public ConfigWindowViewModel(Config Config,ScanConfigPageViewModel scanConfigPageViewModel) {
		this.ScanConfigPageViewModel = scanConfigPageViewModel;
		this.SaveCommand.Subscribe(_ => {
			Config.Save();
		});
	}

	public ReactiveCommand<Unit> SaveCommand {
		get;
	} = new();

	public ScanConfigPageViewModel ScanConfigPageViewModel {
		get;
	}
}
