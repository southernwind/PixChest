using System.Threading.Tasks;
using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Utils.Constants;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.FileTypes.Base.ViewModels;

public abstract class BaseFileViewModel : IFileViewModel {
	protected BaseFileViewModel(IFileModel fileModel) {
		this.FileModel = fileModel;
		this.FilePath = fileModel.FilePath;
		this.ThumbnailFilePath = new($"file:///{fileModel.ThumbnailFilePath ?? FilePathConstants.NoThumbnailFilePath}");
		this.Exists = fileModel.Exists;
		this.Properties = fileModel.Properties;
	}

	private long _thumbnailRefreshTicks = 0;

	public IFileModel FileModel { get; }
	public string FilePath { get; }
	public BindableReactiveProperty<string> ThumbnailFilePath { get; }
	public bool Exists { get; }
	/// <summary>
	/// プロパティ
	/// </summary>
	public Attributes<string> Properties { get; }
	public abstract MediaType MediaType { get; }


	public virtual async Task ExecuteFileAsync() {
		await this.FileModel.ExecuteFileAsync();
	}

	public void RefreshThumbnail() {
		this._thumbnailRefreshTicks = DateTime.Now.Ticks;
		this.ThumbnailFilePath.Value = $"file:///{this.FileModel.ThumbnailFilePath ?? FilePathConstants.NoThumbnailFilePath}?refresh={this._thumbnailRefreshTicks}";
	}
}
