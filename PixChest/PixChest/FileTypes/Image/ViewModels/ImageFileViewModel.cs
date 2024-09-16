using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Image.ViewModels;
public class ImageFileViewModel(IFileModel fileModel) : BaseFileViewModel(fileModel) {
	public override MediaType MediaType {
		get;
	} = MediaType.Image;
}
