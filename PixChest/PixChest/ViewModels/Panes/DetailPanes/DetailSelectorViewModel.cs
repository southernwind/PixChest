using System.Collections.Generic;
using System.Reactive.Linq;

using CommunityToolkit.WinUI.Collections;

using PixChest.Composition.Bases;
using PixChest.Database.Tables;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.FileDetailManagers.Objects;
using PixChest.Utils.Objects;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.DetailPanes;

[AddTransient]
public class DetailSelectorViewModel : ViewModelBase
{
	private bool _isTargetChanging = false;
	public DetailSelectorViewModel(TagsManager tagsManager)
    {
		this.TagCandidates = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(tagsManager.Tags);
		this.LoadTagCandidatesCommand.Subscribe(async _ => await tagsManager.Load());
		this.FilteredTagCandidates = new AdvancedCollectionView(this.TagCandidates) {
			Filter = x => {
				var text = this.Text.Value ?? "";
				if (text.Length == 0) {
					return false;
				}
				if (x is Tag tag) {
					return tag.TagAliases.Select(x => x.Alias).Concat([tag.TagName]).Any(x => x.Contains(text));
				}
				return false;
			}
		};
		this.RefreshFilteredTagCandidatesCommand.Subscribe(x => {
			this.FilteredTagCandidates.RefreshFilter();
		});
		this.TargetFiles.Where(x => x != null).Subscribe(x => {
			this._isTargetChanging = true;
			this.UpdateTags();
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

		this.RemoveTagCommand.Subscribe(async x => {
			await tagsManager.RemoveTag(this.TargetFiles.Value.Select(x => x.FileModel).ToArray(), x.Value);
			this.UpdateTags();
		});

		this.AddTagCommand.Subscribe(async x => {
			if (string.IsNullOrEmpty(this.Text.Value)) {
				return;
			}
			await tagsManager.AddTag(this.TargetFiles.Value.Select(x => x.FileModel).ToArray(), this.Text.Value);
			this.Text.Value = "";
			this.UpdateTags();
		});
	}

	public BindableReactiveProperty<FileViewModel[]> TargetFiles {
		get;
	} = new();

	public BindableReactiveProperty<string> Text {
		get;
	} = new();

	public Reactive.Bindings.ReadOnlyReactiveCollection<Tag> TagCandidates {
		get;
	}

	public IAdvancedCollectionView FilteredTagCandidates {
		get;
	}

	public ReactiveCommand<Unit> RefreshFilteredTagCandidatesCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> LoadTagCandidatesCommand {
		get;
	} = new();

	public Reactive.Bindings.ReactiveCollection<ValueCountPair<string>> Tags {
		get;
	} = [];

	public BindableReactiveProperty<FileProperty[]> Properties {
		get;
	} = new([]);

	public ReactiveCommand<ValueCountPair<string>> RemoveTagCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> AddTagCommand {
		get;
	} = new();

	private void UpdateTags() {
		this.Tags.Clear();
		this.Tags.AddRange(
			this.TargetFiles
				.Value
				.SelectMany(x => x.FileModel.Tags)
				.GroupBy(x => x)
				.Select(x => new ValueCountPair<string>(x.Key, x.Count()))
		);
	}
}
