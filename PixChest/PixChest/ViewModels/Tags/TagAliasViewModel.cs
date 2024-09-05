using PixChest.Database.Tables;

namespace PixChest.ViewModels.Tags;

public class TagAliasViewModel {
	public TagAliasViewModel(TagAlias tagAlias) {
		this.Alias.Value = tagAlias.Alias;
		this.Ruby.Value = tagAlias.Ruby;
	}

	public TagAliasViewModel() {
	}

	public BindableReactiveProperty<string> Alias {
		get;
	} = new();
	public BindableReactiveProperty<string?> Ruby {
		get;
	} = new();
}
