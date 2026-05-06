using MediaDeck.Common.Extensions;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.MediaItemTypes.ViewModels;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.MediaItemTypes.Base.ViewModels;
using MediaDeck.MediaItemTypes.FolderGroup.Models;

namespace MediaDeck.MediaItemTypes.FolderGroup.ViewModels;

/// <summary>
/// フォルダグループ固有の実行設定用 ViewModel。
/// </summary>
[Inject(InjectServiceLifetime.Transient)]
public class FolderGroupExecutionProgramConfigViewModel : DefaultExecutionProgramConfigViewModel, IExecutionProgramConfigViewModel {
	public ExecutionType[] ExecutionTypeConditions { get; } = Enum.GetValues<ExecutionType>();

	// 固有プロパティを ViewModel 直下で公開
	public BindableReactiveProperty<ExecutionType> ExecutionType {
		get;
		private set;
	} = null!;

	public BindableReactiveProperty<bool> IsExternal {
		get;
		private set;
	} = null!;

	public BindableReactiveProperty<bool> IsInternal {
		get;
		private set;
	} = null!;

	public FolderGroupExecutionProgramConfigViewModel(ExecutionConfigModel executionConfig)
		: base(executionConfig) {
	}

	public void Initialize(FolderGroupExecutionProgramObjectModel model) {
		base.Initialize(model);
		this.ExecutionType = model.ExecutionType.ToTwoWayBindableReactiveProperty(Composition.Enum.ExecutionType.External, this.CompositeDisposable).AddTo(this.CompositeDisposable);
		this.IsExternal = this.ExecutionType.Select(x => x == Composition.Enum.ExecutionType.External).ToBindableReactiveProperty(this.ExecutionType.Value == Composition.Enum.ExecutionType.External).AddTo(this.CompositeDisposable);
		this.IsInternal = this.ExecutionType.Select(x => x == Composition.Enum.ExecutionType.Internal).ToBindableReactiveProperty(this.ExecutionType.Value == Composition.Enum.ExecutionType.Internal).AddTo(this.CompositeDisposable);
	}
}