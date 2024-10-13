using System.Reactive.Linq;
using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.Files.Loaders;
using PixChest.Models.Files.SearchConditions;

namespace PixChest.Models.Files;

[AddSingleton]
public class MediaContentLibrary: ModelBase {
	public MediaContentLibrary(FilesLoader filesLoader) {
		this._filesLoader = filesLoader;
		this.SearchConditions.ObserveCountChanged().ThrottleLast(TimeSpan.FromMilliseconds(100)).Subscribe(async _ => await this.SearchAsync());
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

	public async Task SearchAsync() {
		var files = await this._filesLoader.Load(this.SearchConditions);
		this.Files.Clear();
		this.Files.AddRange(files);
	}
}
