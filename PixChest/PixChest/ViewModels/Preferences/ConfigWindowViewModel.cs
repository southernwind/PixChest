using PixChest.Composition.Bases;
using PixChest.Models.Preferences;
using PixChest.ViewModels.Preferences.CustomConfig;
using PixChest.ViewModels.Preferences.CustomConfigs;

namespace PixChest.ViewModels.Preferences;

[AddTransient]
public class ConfigWindowViewModel : ViewModelBase {
	public ConfigWindowViewModel(Config config,ScanConfigPageViewModel scanConfigPageViewModel) {
		this.ScanConfigPageViewModel = scanConfigPageViewModel;

		this.SaveCommand.Subscribe(_ => {
			foreach (var vm in this._configPageViewModels) {
				vm.Save();
			}
			config.Save();
		});

		this.LoadCommand.Subscribe(_ => {
			config.Load();
			foreach (var vm in this._configPageViewModels) {
				vm.Load();
			}
		});
	}
	private IConfigPageViewModel[] _configPageViewModels {
		get {
			return [
				this.ScanConfigPageViewModel
			];
		}
	}

	public ReactiveCommand<Unit> SaveCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> LoadCommand {
		get;
	} = new();

	public ScanConfigPageViewModel ScanConfigPageViewModel {
		get;
	}
}
