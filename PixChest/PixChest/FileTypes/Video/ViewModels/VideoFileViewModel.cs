using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Video.ViewModels;
public class VideoFileViewModel(IFileModel fileModel) : BaseFileViewModel(fileModel) {
	public override MediaType MediaType {
		get;
	} = MediaType.Video;
}
