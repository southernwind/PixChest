using PixChest.Database.Tables;
using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Tags;

public class TagCategoryViewModel {
	public TagCategoryViewModel(TagCategory tagCategory, TagsManager tagsManager) {
		this.TagCategoryId = tagCategory.TagCategoryId;
		this.TagCategoryName.Value = tagCategory.TagCategoryName;
		this.Detail.Value = tagCategory.Detail;
		this.Tags = this._tags.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this._tags.AddRange(tagCategory.Tags.Select(x => new TagViewModel(this, x, tagsManager)));
		this.UpdateTagCategoryCommand = this.TagCategoryName.CombineLatest(this.Detail, (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y)).ToReactiveCommand();
		this.UpdateTagCategoryCommand.Subscribe(async _ => {
			if (tagCategory.TagCategoryId != -1) {
				await tagsManager.UpdateTagCategoryAsync(
					tagCategory.TagCategoryId,
					this.TagCategoryName.Value,
					this.Detail.Value);
			}
			foreach (var tag in this.Tags) {
				tag.UpdateTagCommand.Execute(Unit.Default);
			}
		});
	}

	private readonly ObservableList<TagViewModel> _tags = [];

	public int TagCategoryId {
		get;
	}

	public INotifyCollectionChangedSynchronizedViewList<TagViewModel> Tags {
		get;
	}

	public BindableReactiveProperty<TagViewModel> SelectedTag {
		get;
	} = new();

	public BindableReactiveProperty<string> TagCategoryName {
		get;
	} = new();

	public BindableReactiveProperty<string> Detail {
		get;
	} = new();

	public ReactiveCommand<Unit> UpdateTagCategoryCommand {
		get;
	} = new();
}
