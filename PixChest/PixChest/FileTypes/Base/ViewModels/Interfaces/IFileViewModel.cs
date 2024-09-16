using System.Threading.Tasks;

using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.FileTypes.Base.ViewModels.Interfaces;

public interface IFileViewModel {
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

	public MediaType MediaType {
		get;
	}


	public Task ExecuteFileAsync();
}
