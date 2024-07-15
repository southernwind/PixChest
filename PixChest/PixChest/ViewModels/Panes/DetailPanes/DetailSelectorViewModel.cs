using System.Collections.Specialized;
using System.Reactive.Linq;

using CommunityToolkit.WinUI.Collections;

using PixChest.Composition.Bases;
using PixChest.Models.FileEditors;
using PixChest.ViewModels.Files;

using Reactive.Bindings.Helpers;

namespace PixChest.ViewModels.Panes.DetailPanes;

[AddTransient]
public class DetailSelectorViewModel : ViewModelBase
{
	public DetailSelectorViewModel(TagsManager tagsManager)
    {
		this.TagCandidates = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(tagsManager.TagCandidates, x => x.TagName);
		this.LoadTagCandidatesCommand.Subscribe(async _ => await tagsManager.Load());
		this.FilteredTagCandidates = new AdvancedCollectionView(this.TagCandidates);
		this.FilteredTagCandidates.Filter = x => {
			if (x is string s) {
				return s.Contains(this.Text.Value ?? "");
			}
			return false;
		};
		this.Text.Subscribe(x => {
			this.FilteredTagCandidates.RefreshFilter();
		});
	}

	public BindableReactiveProperty<FileViewModel> TargetFile {
		get;
	} = new();

	public BindableReactiveProperty<string> Text {
		get;
	} = new();

	public Reactive.Bindings.ReadOnlyReactiveCollection<string> TagCandidates {
		get;
	}

	public IAdvancedCollectionView FilteredTagCandidates {
		get;
	}

	public ReactiveCommand<Unit> LoadTagCandidatesCommand {
		get;
	} = new();
}
