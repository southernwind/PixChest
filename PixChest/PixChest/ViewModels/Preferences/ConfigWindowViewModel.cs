using PixChest.Composition.Bases;
using PixChest.Models.Preferences;
using PixChest.ViewModels.Preferences.CustomConfigs;

namespace PixChest.ViewModels.Preferences;

[AddTransient]
public class ConfigWindowViewModel : ViewModelBase {
	public ConfigWindowViewModel(Config config,ScanConfigPageViewModel scanConfigPageViewModel) {
		this.ScanConfigPageViewModel = scanConfigPageViewModel;

		this.SaveCommand.Subscribe(_ => {
			foreach (var vm in this.ConfigPageViewModels) {
				vm.Save();
			}
			config.Save();
		});

		this.LoadCommand.Subscribe(_ => {
			config.Load();
			foreach (var vm in this.ConfigPageViewModels) {
				vm.Load();
			}
		});
	}
	private IConfigPageViewModel[] ConfigPageViewModels {
		get {
			return [
				this.ScanConfigPageViewModel
			];
		}
	}

	public ReactiveCommand SaveCommand {
		get;
	} = new();

	public ReactiveCommand LoadCommand {
		get;
	} = new();

	public ScanConfigPageViewModel ScanConfigPageViewModel {
		get;
	}
}
