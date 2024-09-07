using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Models.Files.FileTypes.Base;
using PixChest.Models.Files.Loaders;

namespace PixChest.Models.Files;

[AddSingleton]
public class MediaContentLibrary(BasicFilesLoader filesLoader):ModelBase {
	private readonly BasicFilesLoader _filesLoader = filesLoader;

	public ObservableList<FileModel> Files {
		get;
	} = [];
	 
	public string? Word {
		get;
		set;
	}

	public async Task SearchAsync() {
		this._filesLoader.Word = this.Word;
		var files = await this._filesLoader.Load();
		this.Files.Clear();
		this.Files.AddRange(files);
	}

}
