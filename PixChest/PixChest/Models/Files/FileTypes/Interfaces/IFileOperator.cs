using System.Threading.Tasks;

using PixChest.Utils.Enums;

namespace PixChest.Models.Files.FileTypes.Interfaces;
public interface IFileOperator {
	public MediaType TargetMediaType {
		get;
	}

	public void RegisterFile(string filepath);

	public Task UpdateRateAsync(long mediaFileId, int rate);

	public Task IncrementUsageCountAsync(long mediaFileId);

	public Task UpdateDescriptionAsync(long mediaFileId, string description);
}
