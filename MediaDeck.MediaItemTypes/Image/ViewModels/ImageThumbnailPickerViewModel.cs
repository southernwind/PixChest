using System.Threading.Tasks;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.MediaItemTypes.Base.Models;
using MediaDeck.MediaItemTypes.Base.ViewModels;
using MediaDeck.MediaItemTypes.Image.Models;

namespace MediaDeck.MediaItemTypes.Image.ViewModels;

[Inject(InjectServiceLifetime.Transient)]
public class ImageThumbnailPickerViewModel(BaseThumbnailPickerModel thumbnailPickerModel, ImageMediaItemOperator ImageMediaItemOperator, IFilePickerService filePickerService, ConfigModel config) : BaseThumbnailPickerViewModel<BaseThumbnailPickerModel>(thumbnailPickerModel, filePickerService, config) {
	private readonly ImageMediaItemOperator _ImageMediaItemOperator = ImageMediaItemOperator;

	public override Task RecreateThumbnailAsync() {
		if (this.targetFileViewModel is null) {
			return Task.CompletedTask;
		}
		this.CandidateThumbnail.Value = this._ImageMediaItemOperator.CreateThumbnail(this.targetFileViewModel.FileModel, (uint)this._config.ThumbnailConfig.ThumbnailSize.Value, (uint)this._config.ThumbnailConfig.ThumbnailSize.Value);
		return Task.CompletedTask;
	}
}