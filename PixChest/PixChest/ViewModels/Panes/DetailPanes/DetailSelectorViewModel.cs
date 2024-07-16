using System.Reactive.Linq;

using CommunityToolkit.WinUI.Collections;

using PixChest.Composition.Bases;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.FileDetailManagers.Objects;
using PixChest.Utils.Objects;
using PixChest.ViewModels.Files;

using Reactive.Bindings.Extensions;

namespace PixChest.ViewModels.Panes.DetailPanes;

[AddTransient]
public class DetailSelectorViewModel : ViewModelBase
{
	private bool _isTargetChanging = false;
	public DetailSelectorViewModel(TagsManager tagsManager)
    {
		this.TagCandidates = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(tagsManager.TagCandidates, x => x.TagName);
		this.LoadTagCandidatesCommand.Subscribe(async _ => await tagsManager.Load());
		this.FilteredTagCandidates = new AdvancedCollectionView(this.TagCandidates) {
			Filter = x => {
				if (x is string s) {
					return s.Contains(this.Text.Value ?? "");
				}
				return false;
			}
		};
		this.Text.Subscribe(x => {
			this.FilteredTagCandidates.RefreshFilter();
		});
		this.Tags.ObserveAddChanged().Where(_ => !this._isTargetChanging).Subscribe(async x => {
			await tagsManager.AddTag(this.TargetFiles.Value.Select(x => x.FileModel).ToArray(), x);
		});
		this.Tags.ObserveRemoveChanged().Where(_=> !this._isTargetChanging).Subscribe(async x => {
			await tagsManager.RemoveTag(this.TargetFiles.Value.Select(x => x.FileModel).ToArray(), x);
		});
		this.TargetFiles.Where(x => x != null).Subscribe(x => {
			this._isTargetChanging = true;
			this.Tags.Clear();
			this.Tags.AddRange(x.SelectMany(x => x.FileModel.Tags).Distinct());
			this.Properties.Value =
				this.TargetFiles.Value
					.SelectMany(x => x.Properties)
					.GroupBy(x => x.Title)
					.Select(x => new FileProperty(
						x.Key,
						x.GroupBy(g => g.Value).Select(g => new ValueCountPair<string?>(g.Key, g.Count()))
					)).ToArray();
			this._isTargetChanging = false;
		});
	}

	public BindableReactiveProperty<FileViewModel[]> TargetFiles {
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

	public Reactive.Bindings.ReactiveCollection<string> Tags {
		get;
	} = [];

	public BindableReactiveProperty<FileProperty[]> Properties {
		get;
	} = new([]);
}
