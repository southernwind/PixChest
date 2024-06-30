using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Models.Files.Loaders;

namespace PixChest.Models.Files;

[AddTransient]
public class MediaContentLibrary(BasicFilesLoader filesLoader):ModelBase {
	private readonly FilesLoader _filesLoader = filesLoader;

	public Reactive.Bindings.ReactiveCollection<FileModel> Files {
		get;
	} = [];

	public async Task Search() {
		var files = await this._filesLoader.Load();
		this.Files.Clear();
		this.Files.AddRangeOnScheduler(files);
	}

}
