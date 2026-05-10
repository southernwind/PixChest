using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.MediaItemTypes.Base.Models;
using MediaDeck.MediaItemTypes.Base.ViewModels;

namespace MediaDeck.MediaItemTypes.Unknown.ViewModels;

[Inject(InjectServiceLifetime.Transient)]
public class UnknownThumbnailPickerViewModel : BaseThumbnailPickerViewModel<BaseThumbnailPickerModel> {
	public UnknownThumbnailPickerViewModel(BaseThumbnailPickerModel thumbnailPickerModel, IFilePickerService filePickerService, ConfigModel config) : base(thumbnailPickerModel, filePickerService, config) { }

	public override void RecreateThumbnail() {
		throw new NotSupportedException("Unknown file type does not support thumbnail creation.");
	}
}