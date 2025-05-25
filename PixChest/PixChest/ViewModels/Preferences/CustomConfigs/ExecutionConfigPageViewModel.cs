using PixChest.Composition.Bases;
using PixChest.Models.Preferences.CustomConfigs;
using PixChest.Models.Preferences.CustomConfigs.Objects;

namespace PixChest.ViewModels.Preferences.CustomConfigs;

[AddTransient]
public class ExecutionConfigPageViewModel : ViewModelBase, IConfigPageViewModel {
	private readonly ExecutionConfig _executionConfig;
	private readonly ObservableList<ExecutionProgramObject> _executionProgramObjects = [];
	public ExecutionConfigPageViewModel(ExecutionConfig executionConfig) {
		this._executionConfig = executionConfig;
		this.AddExecutionProgramCommand.Subscribe(_ => {
			this._executionProgramObjects.Add(new ExecutionProgramObject());
		});
		this.ExecutionProgramObjects = this._executionProgramObjects.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
	}

	/// <summary>
	/// 対象拡張子
	/// </summary>
	public INotifyCollectionChangedSynchronizedViewList<ExecutionProgramObject> ExecutionProgramObjects {
		get;
	}

	public ReactiveCommand AddExecutionProgramCommand {
		get;
	} = new();

	public void Save() {
		this._executionConfig.ExecutionProgramObjects.Clear();
		foreach (var epo in this.ExecutionProgramObjects) {
			this._executionConfig.ExecutionProgramObjects.Add(epo);
		}
	}

	public void Load() {
		this._executionProgramObjects.AddRange(this._executionConfig.ExecutionProgramObjects);
	}
}
