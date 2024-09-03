using PixChest.Composition.Bases;
using PixChest.Models.Preferences.CustomConfigs;
using PixChest.Models.Preferences.CustomConfigs.Objects;
using PixChest.ViewModels.Preferences.CustomConfigs;

namespace PixChest.ViewModels.Preferences.CustomConfig;

[AddTransient]
public class ScanConfigPageViewModel:ViewModelBase, IConfigPageViewModel {
	private readonly ScanConfig _scanConfig;
	public ScanConfigPageViewModel(ScanConfig scanConfig) {
		this._scanConfig = scanConfig;
		this.AddExtensionCommand.Subscribe(_ => {
			scanConfig.TargetExtensions.Add(new ExtensionConfig());
		});
	}

	/// <summary>
	/// 対象拡張子
	/// </summary>
	public Reactive.Bindings.ReactiveCollection<ExtensionConfig> TargetExtensions {
		get;
	} = [];

	public ReactiveCommand<Unit> AddExtensionCommand {
		get;
	} = new();

	public void Save() {
		this._scanConfig.TargetExtensions.Clear();
		this._scanConfig.TargetExtensions.AddRange(this.TargetExtensions);
	}

	public void Load() {
		this.TargetExtensions.AddRange(this._scanConfig.TargetExtensions);
	}
}
