using PixChest.Composition.Bases;
using PixChest.Models.Preferences.CustomConfig;
using PixChest.Models.Preferences.CustomConfig.Objects;

namespace PixChest.ViewModels.Preferences.CustomConfig;

[AddTransient]
public class ScanConfigPageViewModel:ViewModelBase {
	public ScanConfigPageViewModel(ScanConfig scanConfig) {
		this.TargetExtensions = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(scanConfig.TargetExtensions);
		this.AddExtensionCommand.Subscribe(_ => {
			scanConfig.TargetExtensions.Add(new ExtensionConfig());
		});
	}

	/// <summary>
	/// 対象拡張子
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<ExtensionConfig> TargetExtensions {
		get;
	}

	public ReactiveCommand<Unit> AddExtensionCommand {
		get;
	} = new();
}
