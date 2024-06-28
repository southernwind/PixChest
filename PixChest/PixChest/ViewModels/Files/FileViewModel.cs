using PixChest.Models.Files;

namespace PixChest.ViewModels.Files;

[AddTransient]
public class FileViewModel(FileModel fileModel) {
	public string FilePath {
		get;
	} = fileModel.FilePath;

	public string? ThumbnailFilePath {
		get;
	} = fileModel.ThumbnailFilePath;
}
