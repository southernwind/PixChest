using PixChest.Composition.Bases;
using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Tags;
[AddTransient]
public class TagManagerViewModel : ViewModelBase {
	public TagManagerViewModel(TagsManager tagsManager) {
		this.TagCategories = [.. tagsManager.TagCategories.Select(x => new TagCategoryViewModel(x, tagsManager))];
		this.LoadCommand.Subscribe(async _ => await tagsManager.Load());
		this.SaveCommand.Subscribe(async _ => {
			foreach (var tagCategory in this.TagCategories) {
				tagCategory.UpdateTagCategoryCommand.Execute(Unit.Default);
			}
			await tagsManager.Load();
		});
		this.AddTagCategoryCommand.Subscribe(_ => {
			this.TagCategories.Add(new(new() {
				TagCategoryName = "",
				Tags = [],
				Detail = ""
			}, tagsManager));
		});
	}

	public Reactive.Bindings.ReactiveCollection<TagCategoryViewModel> TagCategories {
		get;
	}
	public BindableReactiveProperty<TagCategoryViewModel> SelectedTagCategory {
		get;
	} = new();

	public ReactiveCommand<Unit> LoadCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> SaveCommand {
		get;
	} = new();
	public ReactiveCommand<Unit> AddTagCategoryCommand {
		get;
	} = new();
}
