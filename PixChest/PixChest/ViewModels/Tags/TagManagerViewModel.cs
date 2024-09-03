using PixChest.Composition.Bases;
using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Tags;
[AddTransient]
public class TagManagerViewModel : ViewModelBase {
	public TagManagerViewModel(TagsManager tagsManager) {
		this.Tags = [.. tagsManager.Tags.Select(x => new TagViewModel(x, tagsManager))];
		this.LoadCommand.Subscribe(async _ => await tagsManager.Load());
		this.SaveCommand.Subscribe(async _ => {
			foreach (var tag in this.Tags) {
				tag.UpdateTagCommand.Execute(Unit.Default);
			}
			await tagsManager.Load();
		});
	}

	public Reactive.Bindings.ReactiveCollection<TagViewModel> Tags {
		get;
	}
	public BindableReactiveProperty<TagViewModel> SelectedTag {
		get;
	} = new();

	public ReactiveCommand<Unit> LoadCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> SaveCommand {
		get;
	} = new();
}
