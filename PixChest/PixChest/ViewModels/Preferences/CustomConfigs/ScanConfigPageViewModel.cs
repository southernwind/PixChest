using PixChest.Composition.Bases;
using PixChest.Models.Preferences.CustomConfigs;
using PixChest.Models.Preferences.CustomConfigs.Objects;

namespace PixChest.ViewModels.Preferences.CustomConfigs;

[AddTransient]
public class ScanConfigPageViewModel : ViewModelBase, IConfigPageViewModel {
	private readonly ScanConfig _scanConfig;
	private readonly ObservableList<ExtensionConfig> _targetExtensions = [];
	public ScanConfigPageViewModel(ScanConfig scanConfig) {
		this._scanConfig = scanConfig;
		this.AddExtensionCommand.Subscribe(_ => {
			scanConfig.TargetExtensions.Add(new ExtensionConfig());
		});
		this.TargetExtensions = this._targetExtensions.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
	}

	/// <summary>
	/// 対象拡張子
	/// </summary>
	public INotifyCollectionChangedSynchronizedViewList<ExtensionConfig> TargetExtensions {
		get;
	}

	public ReactiveCommand<Unit> AddExtensionCommand {
		get;
	} = new();

	public void Save() {
		this._scanConfig.TargetExtensions.Clear();
		this._scanConfig.TargetExtensions.AddRange(this.TargetExtensions);
	}

	public void Load() {
		this._targetExtensions.AddRange(this._scanConfig.TargetExtensions);
	}
}
