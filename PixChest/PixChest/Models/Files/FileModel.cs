using PixChest.Composition.Bases;

namespace PixChest.Models.Files;
public class FileModel(string filePath):ModelBase {
	public string FilePath {
		get;
	} = filePath;

	public string? ThumbnailFilePath {
		get;
		init;
	}
}
