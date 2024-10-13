using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Models.Files;
using PixChest.Models.Files.SearchConditions;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddSingleton]
public class MediaContentLibraryViewModel : ViewModelBase {
	public MediaContentLibraryViewModel(MediaContentLibrary mediaContentLibrary) {
		this._mediaContentLibrary = mediaContentLibrary;
		this.Files = mediaContentLibrary.Files.CreateView(FileTypeUtility.CreateFileViewModel).ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);
		this.SearchConditions =
			mediaContentLibrary
				.SearchConditions
				.ToNotifyCollectionChanged(x =>
					new SearchConditionViewModel(x),
					SynchronizationContextCollectionEventDispatcher.Current);
	}

	private readonly MediaContentLibrary _mediaContentLibrary;

	public INotifyCollectionChangedSynchronizedViewList<IFileViewModel> Files {
		get;
	}

	public INotifyCollectionChangedSynchronizedViewList<SearchConditionViewModel> SearchConditions {
		get;
	}

	public BindableReactiveProperty<IFileViewModel> SelectedFile {
		get;
	} = new();

	public BindableReactiveProperty<IFileViewModel[]> SelectedFiles {
		get;
	} = new();

	public void AddWordSearchCondition(string word) {
		this._mediaContentLibrary.SearchConditions.Add(new WordSearchCondition(word));
	}

	public void RemoveSearchCondition(SearchConditionViewModel searchCondition) {
		this._mediaContentLibrary.SearchConditions.Remove(searchCondition.SearchCondition);
	}
}
