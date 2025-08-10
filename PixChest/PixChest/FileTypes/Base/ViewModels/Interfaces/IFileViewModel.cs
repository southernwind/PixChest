using System.Threading.Tasks;
using MapControl;
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

	public BindableReactiveProperty<string> ThumbnailFilePath {
		get;
	}

	public bool Exists {
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

	public Location? Location {
		get;
	}

	public Task ExecuteFileAsync();

	public void RefreshThumbnail();
}
