using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces.MediaItemTypes;
using MediaDeck.Composition.Interfaces.MediaItemTypes.ViewModels;

namespace MediaDeck.ViewModels.Thumbnails;

[Inject(InjectServiceLifetime.Transient)]
public class ThumbnailPickerSelectorViewModel : ViewModelBase {
	public ThumbnailPickerSelectorViewModel(IMediaItemTypeService MediaItemTypeService) {
		this.FileViewModel.SubscribeAwait(async (x, ct) => {
			if (x == null) {
				this.ThumbnailPickerViewModel.Value = null;
				return;
			}
			var thumbnailPickerViewModel = MediaItemTypeService.CreateThumbnailPickerViewModel(x);
			await thumbnailPickerViewModel.LoadAsync(x);
			this.ThumbnailPickerViewModel.Value = thumbnailPickerViewModel;
		}, AwaitOperation.Switch).AddTo(this.CompositeDisposable);
	}


	public BindableReactiveProperty<IThumbnailPickerViewModel?> ThumbnailPickerViewModel {
		get;
	} = new();

	public BindableReactiveProperty<IMediaItemViewModel> FileViewModel {
		get;
	} = new();
}