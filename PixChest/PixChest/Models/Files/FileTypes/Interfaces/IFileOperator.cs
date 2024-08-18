using PixChest.Utils.Enums;

namespace PixChest.Models.Files.FileTypes.Interfaces;
public interface IFileOperator {
	public MediaType TargetMediaType {
		get;
	}

	public void RegisterFile(string filepath);
}
