using System.Reactive.Linq;
using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.Files.Loaders;
using PixChest.Models.Files.SearchConditions;
using PixChest.Models.NotificationDispatcher;
using PixChest.Models.Repositories;

namespace PixChest.Models.Files;

[AddSingleton]
public class MediaContentLibrary: ModelBase {
	public MediaContentLibrary(FilesLoader filesLoader, SearchConditionNotificationDispatcher searchConditionNotificationDispatcher,TagsManager tagsManager, FolderRepository folderRepository) {
		this._filesLoader = filesLoader;
		this.SearchConditions.ObserveChanged().ThrottleLast(TimeSpan.FromMilliseconds(100)).Subscribe(async _ => await this.SearchAsync());
		tagsManager.Load().Wait();
		this.SearchConditionCandidates.AddRange(tagsManager.Tags.Select(x => new TagSearchCondition(x) as ISearchCondition));
		this.SearchConditionCandidates.AddRange(folderRepository.GetAllFolders().Select(x => new FolderSearchCondition(x) as ISearchCondition));
		searchConditionNotificationDispatcher.AddRequest.Subscribe(this.SearchConditions.Add);
		searchConditionNotificationDispatcher.RemoveRequest.Subscribe(x => this.SearchConditions.Remove(x));
		searchConditionNotificationDispatcher.UpdateRequest.Subscribe(x => x(this.SearchConditions));
	}
	private readonly FilesLoader _filesLoader;

	public ObservableList<IFileModel> Files {
		get;
	} = [];
	 
	public string? Word {
		get;
		set;
	}

	public ObservableList<ISearchCondition> SearchConditions {
		get;
	} = [];

	public ObservableList<ISearchCondition> SearchConditionCandidates {
		get;
	} = [];

	public async Task SearchAsync() {
		var files = await this._filesLoader.Load(this.SearchConditions);
		this.Files.Clear();
		this.Files.AddRange(files);
	}
}
