using GenJsonConfig.Attributes;

namespace MediaDeck.Composition.Stores.Config.Model.Objects;

[Inject(InjectServiceLifetime.Transient)]
[GenerateJsonConfigDto]
public class FolderModel() {
	public string FolderPath {
		get;
		set;
	} = string.Empty;

	public bool IsGroupingRoot {
		get;
		set;
	}

	[ExcludeProperty]
	public ReactiveProperty<bool> IsScanning {
		get;
	} = new(false);

	[ExcludeProperty]
	public ReactiveProperty<long> TotalCount {
		get;
	} = new(0);

	[ExcludeProperty]
	public ReactiveProperty<long> RemainingCount {
		get;
	} = new(0);
}