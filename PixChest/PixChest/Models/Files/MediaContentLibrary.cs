using System.IO;
using System.Threading.Tasks;

using PixChest.ViewModels.Files;

namespace PixChest.Models.Files; 
public class MediaContentLibrary {
	public ReactiveCollection<FileModel> Files {
		get;
	} = [];

	public async Task Search() {
		await Task.Run(() => {
			var files = Directory.EnumerateFiles(@"C:\Users\admin\Pictures", "", SearchOption.AllDirectories);
			this.Files.Clear();
			this.Files.AddRangeOnScheduler(files.Select(x => new FileModel(x)));
		});
	}

}
