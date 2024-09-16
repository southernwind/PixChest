using System.Diagnostics;
using System.Threading.Tasks;

using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Utils.Constants;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.FileTypes.Base.ViewModels;

public abstract class BaseFileViewModel: IFileViewModel {
	public BaseFileViewModel(IFileModel fileModel) {
		this.FileModel = fileModel;
		this.FilePath = fileModel.FilePath;
		this.ThumbnailFilePath = fileModel.ThumbnailFilePath ?? FilePathConstants.NoThumbnailFilePath;
		this.Properties = fileModel.Properties;
	}
	public IFileModel FileModel {
		get;
	}

	public string FilePath {
		get;
	}

	public string ThumbnailFilePath {
		get;
	}

	/// <summary>
	/// プロパティ
	/// </summary>
	public Attributes<string> Properties {
		get;
	}

	public abstract MediaType MediaType {
		get;
	}

	public virtual async Task ExecuteFileAsync() {
		var psi = new ProcessStartInfo {
			FileName = this.FilePath,
			UseShellExecute = true
		};
		_ = Process.Start(psi);
		await this.FileModel.IncrementUsageCountAsync();
	}
}
