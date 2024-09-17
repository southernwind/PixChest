using System.Reactive.Linq;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.FileDetailManagers.Objects;
using PixChest.Utils.Objects;
using PixChest.ViewModels.Panes.ViewerPanes;

namespace PixChest.ViewModels.Panes.DetailPanes;

[AddTransient]
public class DetailSelectorViewModel : ViewModelBase
{
	private bool _isTargetChanging = false;
	public DetailSelectorViewModel(TagsManager tagsManager, MediaContentLibraryViewModel mediaContentLibraryViewModel)
    {
		this.TagCandidates = tagsManager.TagsWithKanaRomajiAliases.CreateView(x => x);
		this.LoadTagCandidatesCommand.Subscribe(async _ => await tagsManager.Load());
		this.FilteredTagCandidates = this.TagCandidates.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this.RefreshFilteredTagCandidatesCommand.Subscribe(x => {
			this.RefreshTagCandidateFilter();
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
			if (this.TargetFiles.Value.Length == 1) {
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

		this.Tags = this._tags.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
	}

	private readonly ObservableList<ValueCountPair<string>> _tags = [];

	public BindableReactiveProperty<IFileViewModel[]> TargetFiles {
		get;
	} = new();

	/// <summary>
	/// 代表ファイル 複数選択時はnull
	/// </summary>
	public BindableReactiveProperty<IFileViewModel?> RepresentativeFile {
		get;
	} = new();

	public BindableReactiveProperty<string> Text {
		get;
	} = new();

	public ISynchronizedView<TagWithRomaji, TagWithRomaji> TagCandidates {
		get;
	}

	public INotifyCollectionChangedSynchronizedViewList<TagWithRomaji> FilteredTagCandidates {
		get;
	}

	public ReactiveCommand<Unit> RefreshFilteredTagCandidatesCommand {
		get;
	} = new();

	public ReactiveCommand<Unit> LoadTagCandidatesCommand {
		get;
	} = new();

	public INotifyCollectionChangedSynchronizedViewList<ValueCountPair<string>> Tags {
		get;
	}

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
		this._tags.Clear();
		this._tags.AddRange(
			this.TargetFiles
				.Value
				.SelectMany(x => x.FileModel.Tags)
				.GroupBy(x => x)
				.Select(x => new ValueCountPair<string>(x.Key, x.Count()))
		);
	}

	private void RefreshTagCandidateFilter() {
		this.TagCandidates.AttachFilter(tag => {
			var text = this.Text.Value ?? "";
			if (text.Length == 0) {
				return false;
			}
			if (tag.TagName.Contains(text)) {
				tag.RepresentativeText.Value = null;
				return true;
			}
			var result =
				tag
					.TagAliases
					.FirstOrDefault(
						x =>
							x.Alias.Contains(text) ||
							(x.Ruby?.Contains(text) ?? false) ||
							(x.Romaji?.Contains(text) ?? false)
					);
			tag.RepresentativeText.Value = result?.Alias;
			return result != null;
		});
	}
}
