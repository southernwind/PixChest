using System.Threading.Tasks;
using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Utils.Constants;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.FileTypes.Base.ViewModels;

public abstract class BaseFileViewModel(IFileModel fileModel) : IFileViewModel {
	private long _thumbnailRefreshTicks = 0;

	public IFileModel FileModel {
		get;
	} = fileModel;

	public string FilePath {
		get;
	} = fileModel.FilePath;

	public BindableReactiveProperty<string> ThumbnailFilePath {
		get;
	} = new($"file:///{fileModel.ThumbnailFilePath ?? FilePathConstants.NoThumbnailFilePath}");

	public bool Exists {
		get;
	} = fileModel.Exists;

	/// <summary>
	/// プロパティ
	/// </summary>
	public Attributes<string> Properties {
		get;
	} = fileModel.Properties;

	public abstract MediaType MediaType {
		get;
	}

	public virtual async Task ExecuteFileAsync() {
		await this.FileModel.ExecuteFileAsync();
	}

	public void RefreshThumbnail() {
		this._thumbnailRefreshTicks = DateTime.Now.Ticks;
		this.ThumbnailFilePath.Value = $"file:///{fileModel.ThumbnailFilePath ?? FilePathConstants.NoThumbnailFilePath}?refresh={this._thumbnailRefreshTicks}";
	}
}
