using MediaDeck.Common.Base;
using MediaDeck.Core.Models.FolderManager;

namespace MediaDeck.ViewModels.FolderManager;

[Inject(InjectServiceLifetime.Transient)]
public class FolderManagerViewModel : ViewModelBase {
	private readonly FolderManagerModel _folderManager;

	public INotifyCollectionChangedSynchronizedViewList<FolderViewModel> Folders {
		get;
	}

	public BindableReactiveProperty<FolderViewModel?> SelectedFolder {
		get;
	} = new();

	/// <summary>
	/// 登録フォルダ一覧が空であるかどうかを取得します。
	/// </summary>
	public BindableReactiveProperty<bool> IsEmpty {
		get;
	}

	/// <summary>
	/// フォルダ未選択かつ登録フォルダ数が0のときに、フォルダ登録を促すメッセージを表示するかどうかを取得します。
	/// </summary>
	public BindableReactiveProperty<bool> ShowEmptyMessage {
		get;
	}

	/// <summary>
	/// フォルダ未選択かつ登録フォルダ数が1以上のときに、フォルダ選択を促すメッセージを表示するかどうかを取得します。
	/// </summary>
	public BindableReactiveProperty<bool> ShowSelectFolderMessage {
		get;
	}

	public ReactiveCommand<string> AddFolderCommand {
		get;
	} = new();

	public ReactiveCommand<FolderViewModel> RemoveFolderCommand {
		get;
	} = new();

	public ReactiveCommand ScanCommand {
		get;
	} = new();

	public ReactiveCommand<FolderViewModel> ScanSelectedFolderCommand {
		get;
	} = new();

	public FolderManagerViewModel(FolderManagerModel folderManager) {
		this._folderManager = folderManager;
		this.Folders = this._folderManager.Folders.CreateView(x => new FolderViewModel(x)).ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);

		this.IsEmpty = this._folderManager.Folders
			.ObserveCountChanged()
			.Select(count => count == 0)
			.ToBindableReactiveProperty(this._folderManager.Folders.Count == 0)
			.AddTo(this.CompositeDisposable);

		this.ShowEmptyMessage = this.SelectedFolder
			.CombineLatest(this.IsEmpty, (selected, isEmpty) => selected is null && isEmpty)
			.ObserveOnCurrentSynchronizationContext()
			.ToBindableReactiveProperty(this.SelectedFolder.Value is null && this.IsEmpty.Value)
			.AddTo(this.CompositeDisposable);

		this.ShowSelectFolderMessage = this.SelectedFolder
			.CombineLatest(this.IsEmpty, (selected, isEmpty) => selected is null && !isEmpty)
			.ObserveOnCurrentSynchronizationContext()
			.ToBindableReactiveProperty(this.SelectedFolder.Value is null && !this.IsEmpty.Value)
			.AddTo(this.CompositeDisposable);

		this.AddFolderCommand.Subscribe(x => this._folderManager.AddFolder(x)).AddTo(this.CompositeDisposable);
		this.RemoveFolderCommand.Subscribe(x => this._folderManager.RemoveFolder(x.GetModel())).AddTo(this.CompositeDisposable);
		this.ScanCommand.Subscribe(async x => await this._folderManager.Scan()).AddTo(this.CompositeDisposable);
		this.ScanSelectedFolderCommand.Subscribe(async x => {
			if (x is not null) {
				await this._folderManager.ScanFolder(x.GetModel());
			}
		})
			.AddTo(this.CompositeDisposable);
	}
}