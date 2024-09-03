using System.Collections.Generic;
using System.Reactive.Linq;

using CommunityToolkit.WinUI.Collections;

using PixChest.Composition.Bases;
using PixChest.Database.Tables;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.FileDetailManagers.Objects;
using PixChest.Models.Files;
using PixChest.Models.Files.FileTypes.Base;
using PixChest.Utils.Objects;
using PixChest.ViewModels.Files;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels.Panes.DetailPanes;

[AddTransient]
public class DetailSelectorViewModel : ViewModelBase
{
	private bool _isTargetChanging = false;
	public DetailSelectorViewModel(TagsManager tagsManager, MediaContentLibraryViewModel mediaContentLibraryViewModel)
    {
		this.TagCandidates = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(tagsManager.TagsWithKanaRomajiAliases);
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
			if (this.TargetFiles.Value.Length > 0) {
				this.UpdateTags();
				this.Properties.Value =
					this.TargetFiles.Value
						.SelectMany(x => x.Properties)
						.GroupBy(x => x.Title)
						.Select(x => new FileProperty(
							x.Key,
							x.GroupBy(g => g.Value).Select(g => new ValueCountPair<string?>(g.Key, g.Count()))
						)).ToArray();
				this.Rate.Value = this.TargetFiles.Value.Average(x => x.FileModel.Rate);
				this.UsageCount.Value = this.TargetFiles.Value.Average(x => x.FileModel.UsageCount);
			}
			if (this.TargetFiles.Value.Count() == 1) {
				this.RepresentativeFile.Value = this.TargetFiles.Value.First();
				this.Description.Value = this.RepresentativeFile.Value.FileModel.Description;
			} else {
				this.RepresentativeFile.Value = null;
				this.Description.Value = string.Empty;
			}
			this._isTargetChanging = false;
		});

		this.UpdateDescriptionCommnd.Subscribe(async _ => {
			await this.TargetFiles.Value.First().FileModel.UpdateDescriptionAsync(this.Description.Value);
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
		this.SearchTaggedFilesCommand.Subscribe(x => {
			mediaContentLibraryViewModel.SearchWord.Value = x.Value;
			mediaContentLibraryViewModel.ReloadCommand.Execute(Unit.Default);
		});
		this.Rate.Subscribe(async x => {
			if(this._isTargetChanging) {
				return;
			}
			if (!double.IsInteger(x)) {
				return;
			}
			if (this.TargetFiles.Value is null) {
				return;
			}
			foreach (var file in this.TargetFiles.Value) {
				await file.FileModel.UpdateRateAsync((int)x);
			}
		});
	}

	public BindableReactiveProperty<FileViewModel[]> TargetFiles {
		get;
	} = new();

	/// <summary>
	/// 代表ファイル 複数選択時はnull
	/// </summary>
	public BindableReactiveProperty<FileViewModel?> RepresentativeFile {
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

	public ReactiveCommand<ValueCountPair<string>> SearchTaggedFilesCommand {
		get;
	} = new();

	public BindableReactiveProperty<FileProperty[]> Properties {
		get;
	} = new([]);

	/// <summary>
	/// 評価
	/// </summary>
	public BindableReactiveProperty<double> Rate {
		get;
	} = new();

	public BindableReactiveProperty<string> Description {
		get;
	} = new();

	public BindableReactiveProperty<double> UsageCount {
		get;
	} = new();

	public ReactiveCommand<Unit> UpdateDescriptionCommnd {
		get;
	} = new();

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
