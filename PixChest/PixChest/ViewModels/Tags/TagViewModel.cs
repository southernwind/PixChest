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
		this.TagCategoryCandidates = tagsManager.TagCategories;
		this.TagCategory.Value = this.TagCategoryCandidates.FirstOrDefault(x => x.TagCategoryId == (tag.TagCategoryId ?? -1));
		this.UpdateTagCommand = this.TagName.CombineLatest(this.Detail, (x,y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y)).ToReactiveCommand();
		this.UpdateTagCommand.Subscribe(async _ => {
			if (!this._editedFlag) {
				return;
			}
			await tagsManager.UpdateTag(
				tag.TagId,
				this.TagCategory.Value?.TagCategoryId,
				this.TagName.Value,
				this.Detail.Value,
				this.TagAliases.Select(x =>
					new TagAlias() {
						Alias = x.Alias.Value,
						Ruby = string.IsNullOrEmpty(x.Ruby.Value) ? null : x.Ruby.Value
					}));
			this._editedFlag = false;
		});
		this.RemoveTagAliasCommand.Subscribe(x => {
			this.TagAliases.Remove(x);
		});
		this.AddTagAliasCommand.Subscribe(_ => {
			this.TagAliases.Add(new());
		});

		this.TagName
			.Select(_ => Unit.Default)
			.Concat(this.Detail.Select(_ => Unit.Default))
			.Concat(Reactive.Bindings.Extensions.INotifyCollectionChangedExtensions.CollectionChangedAsObservable(this.TagAliases).ToObservable().Select(_ => Unit.Default))
			.Concat(this.TagCategory.Select(_ => Unit.Default))
			.Subscribe(_ => {
			this._editedFlag = true;
		});
		this._editedFlag = false;
	}

	private bool _editedFlag = false;

	public BindableReactiveProperty<string> TagName {
		get;
	} = new();

	public BindableReactiveProperty<string> Detail {
		get;
	} = new();

	public BindableReactiveProperty<TagCategory?> TagCategory {
		get;
	} = new();

	public Reactive.Bindings.ReactiveCollection<TagCategory> TagCategoryCandidates {
		get;
	}

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
