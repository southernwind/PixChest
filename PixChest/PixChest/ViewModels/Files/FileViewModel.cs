using System.Diagnostics;
using System.Threading.Tasks;

using PixChest.FileTypes.Models.Files;
using PixChest.Utils.Constants;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Files;

[AddTransient]
public class FileViewModel {
	public FileViewModel(FileModel fileModel) {
		this.FileModel= fileModel;
		this.FilePath = fileModel.FilePath;
		this.ThumbnailFilePath = fileModel.ThumbnailFilePath ?? FilePathConstants.NoThumbnailFilePath;
		this.Properties = fileModel.Properties;
	}
	public FileModel FileModel {
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

	public MediaType MediaType {
		get {
			switch (this.FileModel) {
				case ImageFileModel:
					return MediaType.Image;
				case VideoFileModel:
					return MediaType.Video;
					case PdfFileModel:
					return MediaType.Pdf;
				default:
					throw new Exception();
			}
		}
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
