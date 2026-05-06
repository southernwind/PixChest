using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces.MediaItemTypes;
using MediaDeck.Composition.Interfaces.MediaItemTypes.ViewModels;
using MediaDeck.Composition.Interfaces.Notifications;
using MediaDeck.Core.Models.Files;
using MediaDeck.Core.Models.Repositories;

namespace MediaDeck.ViewModels.Panes.ViewerPanes;

[Inject(InjectServiceLifetime.Scoped)]
public class MediaContentLibraryViewModel : ViewModelBase {
	private readonly AlbumRepository _albumRepository;

	public MediaContentLibraryViewModel(MediaContentLibrary mediaContentLibrary, SearchConditionManagerViewModel searchConditionManagerViewModel, IMediaItemTypeService MediaItemTypeService, AlbumRepository albumRepository) {
		this._albumRepository = albumRepository;
		this.SearchConditionManagerViewModel = searchConditionManagerViewModel;
		this.Files = mediaContentLibrary.Files.CreateView(MediaItemTypeService.CreateMediaItemViewModel).ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this.SearchElapsedMilliseconds = mediaContentLibrary.SearchElapsedMilliseconds.ObserveOnCurrentSynchronizationContext().ToBindableReactiveProperty().AddTo(this.CompositeDisposable);
	}

	public SearchConditionManagerViewModel SearchConditionManagerViewModel {
		get;
	}

	public NotifyCollectionChangedSynchronizedViewList<IMediaItemViewModel> Files {
		get;
	}

	public INotifyCollectionChangedSynchronizedViewList<SearchConditionViewModel> SearchConditions {
		get {
			return this.SearchConditionManagerViewModel.SearchConditions;
		}
	}

	public INotifyCollectionChangedSynchronizedViewList<SearchConditionViewModel> FilteredSearchConditionCandidates {
		get {
			return this.SearchConditionManagerViewModel.FilteredSearchConditionCandidates;
		}
	}

	public BindableReactiveProperty<IMediaItemViewModel> SelectedFile {
		get;
	} = new();

	public BindableReactiveProperty<IMediaItemViewModel[]> SelectedFiles {
		get;
	} = new([]);

	public BindableReactiveProperty<long?> SearchElapsedMilliseconds {
		get;
	}

	public ISearchConditionNotificationDispatcher SearchConditionNotificationDispatcher {
		get {
			return this.SearchConditionManagerViewModel.SearchConditionNotificationDispatcher;
		}
	}

	public void RefreshSearchTokenCandidates(string word) {
		this.SearchConditionManagerViewModel.RefreshSearchTokenCandidates(word);
	}

	public void Reload() {
		this.SearchConditionManagerViewModel.Reload();
	}

	public async Task<IReadOnlyList<string>> GetRecentAlbumPathsAsync(int max = 12) {
		var albums = await this._albumRepository.GetRecentAlbumsAsync(max);
		return albums.Select(x => x.Path).ToList();
	}

	public async Task AddToAlbumAsync(string albumPath, IEnumerable<IMediaItemViewModel> mediaItems) {
		var ids = mediaItems.Select(x => x.FileModel.Id).Distinct().ToArray();
		if (ids.Length == 0) {
			return;
		}
		await this._albumRepository.AddItemsAsync(albumPath, ids);
	}
}