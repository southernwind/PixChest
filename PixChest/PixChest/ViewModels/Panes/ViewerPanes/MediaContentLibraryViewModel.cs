using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Models.Files;
using PixChest.Models.Files.SearchConditions;
using PixChest.Models.NotificationDispatcher;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddSingleton]
public class MediaContentLibraryViewModel : ViewModelBase {
	public MediaContentLibraryViewModel(MediaContentLibrary mediaContentLibrary, SearchConditionNotificationDispatcher searchConditionNotificationDispatcher) {
		this._mediaContentLibrary = mediaContentLibrary;
		this.Files = mediaContentLibrary.Files.CreateView(FileTypeUtility.CreateFileViewModel).ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this.SearchConditions =
			mediaContentLibrary
				.SearchConditions
				.ToWritableNotifyCollectionChanged(
					x => new SearchConditionViewModel(x),
					(SearchConditionViewModel scvm, ISearchCondition sc,ref bool setValue) => scvm.SearchCondition,
					SynchronizationContextCollectionEventDispatcher.Current);

		this.SearchConditionCandidates = this._mediaContentLibrary.SearchConditionCandidates.CreateView(x => new SearchConditionViewModel(x));
		this.FilteredSearchConditionCandidates = this.SearchConditionCandidates.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);

		this.SearchConditionNotificationDispatcher = searchConditionNotificationDispatcher;
	}

	private readonly MediaContentLibrary _mediaContentLibrary;

	public NotifyCollectionChangedSynchronizedViewList<IFileViewModel> Files {
		get;
	}

	public INotifyCollectionChangedSynchronizedViewList<SearchConditionViewModel> SearchConditions {
		get;
	}

	public ISynchronizedView<ISearchCondition, SearchConditionViewModel> SearchConditionCandidates {
		get;
	}

	public INotifyCollectionChangedSynchronizedViewList<SearchConditionViewModel> FilteredSearchConditionCandidates {
		get;
	}

	public BindableReactiveProperty<IFileViewModel> SelectedFile {
		get;
	} = new();

	public BindableReactiveProperty<IFileViewModel[]> SelectedFiles {
		get;
	} = new();

	public SearchConditionNotificationDispatcher SearchConditionNotificationDispatcher {
		get;
	}

	public void RefreshSearchTokenCandidates(string word) {
		this.SearchConditionCandidates.AttachFilter(x => {
			return x.IsMatchForSuggest(word);
		});
	}
}
