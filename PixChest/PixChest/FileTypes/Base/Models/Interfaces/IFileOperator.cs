using System.Threading.Tasks;

using PixChest.Database.Tables;
using PixChest.Utils.Enums;

namespace PixChest.FileTypes.Base.Models.Interfaces;
public interface IFileOperator {
	public MediaType TargetMediaType {
		get;
	}

	public Task<MediaFile?> RegisterFileAsync(string filePath);

	public Task<MediaFile?> UpdateRateAsync(long mediaFileId, int rate);

	public Task<MediaFile?> IncrementUsageCountAsync(long mediaFileId);

	public Task<MediaFile?> UpdateDescriptionAsync(long mediaFileId, string description);
}
