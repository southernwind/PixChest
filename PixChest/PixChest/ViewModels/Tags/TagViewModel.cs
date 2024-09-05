using PixChest.Composition.Bases;
using PixChest.Database.Tables;
using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Tags;
[AddTransient]
public class TagViewModel : ViewModelBase {
	public TagViewModel(Tag tag, TagsManager tagsManager) {
		this.TagName.Value = tag.TagName;
		this.Detail.Value = tag.Detail;
		this.TagAliases.AddRange(tag.TagAliases.Select(x => new TagAliasViewModel(x)));
		this.UpdateTagCommand = this.TagName.CombineLatest(this.Detail, (x,y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y)).ToReactiveCommand();
		this.UpdateTagCommand.Subscribe(async _ =>
			await tagsManager.UpdateTag(
				tag.TagId,
				this.TagName.Value,
				this.Detail.Value,
				this.TagAliases.Select(	x =>
					new TagAlias() {
						Alias = x.Alias.Value,
						Ruby = string.IsNullOrEmpty(x.Ruby.Value) ? null : x.Ruby.Value
					})));
		this.RemoveTagAliasCommand.Subscribe(x => {
			this.TagAliases.Remove(x);
		});
		this.AddTagAliasCommand.Subscribe(_ => {
			this.TagAliases.Add(new());
		});
	}

	public BindableReactiveProperty<string> TagName {
		get;
	} = new();

	public BindableReactiveProperty<string> Detail {
		get;
	} = new();

	public Reactive.Bindings.ReactiveCollection<TagAliasViewModel> TagAliases {
		get;
	} = [];

	public ReactiveCommand<Unit> UpdateTagCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> AddTagAliasCommand {
		get;
	} = new();

	public ReactiveCommand<TagAliasViewModel> RemoveTagAliasCommand {
		get;
	} = new();
}
