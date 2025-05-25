using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Archive.ViewModels;
public class ArchiveFileViewModel(IFileModel fileModel) : BaseFileViewModel(fileModel) {
	public override MediaType MediaType {
		get;
	} = MediaType.Archive;
}
