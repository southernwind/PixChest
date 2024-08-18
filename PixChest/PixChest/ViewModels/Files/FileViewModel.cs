using System.Diagnostics;
using System.Threading.Tasks;
using PixChest.Models.Files.FileTypes.Base;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Files;

[AddTransient]
public class FileViewModel {
	public FileViewModel(FileModel fileModel) {
		this.FileModel= fileModel;
		this.FilePath = fileModel.FilePath;
		this.ThumbnailFilePath = fileModel.ThumbnailFilePath;
		this.Properties = fileModel.Properties;
	}
	public FileModel FileModel {
		get;
	}

	public string FilePath {
		get;
	}

	public string? ThumbnailFilePath {
		get;
	}

	/// <summary>
	/// プロパティ
	/// </summary>
	public Attributes<string> Properties {
		get;
	}

	public virtual Task ExecuteFile() {
		var psi = new ProcessStartInfo {
			FileName = this.FilePath,
			UseShellExecute = true
		};
		_ = Process.Start(psi);
		return Task.CompletedTask;
	}
}
