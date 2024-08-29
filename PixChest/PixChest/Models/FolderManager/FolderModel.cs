using PixChest.Models.Files;

using System.IO;
using System.Threading.Tasks;

namespace PixChest.Models.FolderManager;
public class FolderModel(string folderPath, FileRegistrar fileRegistrar) {
	private readonly FileRegistrar _fileRegistrar = fileRegistrar;
	public string FolderPath {
		get;
	} = folderPath;

	public async Task Scan() {
		var files = Directory.EnumerateFiles(this.FolderPath, "", SearchOption.AllDirectories);
		await Task.Run(() => {
			this._fileRegistrar.RegistrationQueue.EnqueueRange(files.Where(x => x.IsTargetFile()));
		});
	}
}
