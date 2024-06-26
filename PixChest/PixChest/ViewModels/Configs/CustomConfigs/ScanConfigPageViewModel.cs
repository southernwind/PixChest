using PixChest.Composition.Bases;
using PixChest.Models.Preferences.CustomConfig;
using PixChest.Models.Preferences.CustomConfig.Objects;

namespace PixChest.ViewModels.Preferenses.CustomConfig;

public class ScanConfigPageViewModel:ViewModelBase {
	public ScanConfigPageViewModel(ScanConfig scanConfig) {
		this.TargetExtensions = scanConfig.TargetExtensions.ToReadOnlyReactiveCollection();
		this.AddExtensionCommand.Subscribe(() => {
			scanConfig.TargetExtensions.Add(new ExtensionConfig());
		});
	}

	/// <summary>
	/// 対象拡張子
	/// </summary>
	public ReadOnlyReactiveCollection<ExtensionConfig> TargetExtensions {
		get;
	}

	public ReactiveCommand AddExtensionCommand {
		get;
	} = new();
}
