using System.Threading.Tasks;

using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Base.Models.Interfaces;
public interface IFileOperator {
	public MediaType TargetMediaType {
		get;
	}

	public Task RegisterFileAsync(string filePath);

	public Task UpdateRateAsync(long mediaFileId, int rate);

	public Task IncrementUsageCountAsync(long mediaFileId);

	public Task UpdateDescriptionAsync(long mediaFileId, string description);
}
