using PixChest.Models.Files;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Files;

[AddTransient]
public class FileViewModel(FileModel fileModel) {
	public string FilePath {
		get;
	} = fileModel.FilePath;

	public string? ThumbnailFilePath {
		get;
	} = fileModel.ThumbnailFilePath;

	/// <summary>
	/// プロパティ
	/// </summary>
	public virtual Attributes<string> Properties {
		get;
	} = fileModel.Properties;
}
