using GenJsonConfig.Attributes;
using MediaDeck.Composition.Enum;

namespace MediaDeck.Composition.Stores.Config.Model.Objects;

[Inject(InjectServiceLifetime.Transient)]
[GenerateJsonConfigDto]
public class ExtensionObjectModel {
	public ReactiveProperty<string> Extension {
		get;
		set;
	} = new();

	public ReactiveProperty<MediaType> MediaType {
		get;
		set;
	} = new();
}