using PixChest.FileTypes.Base.Models;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Archive.Models;
public class ArchiveFileModel(long id, string filePath) : BaseFileModel(id, filePath, _fileOperator) {
	private static readonly ArchiveFileOperator _fileOperator = new();
	public override MediaType MediaType {
		get;
	} = MediaType.Archive;
}
