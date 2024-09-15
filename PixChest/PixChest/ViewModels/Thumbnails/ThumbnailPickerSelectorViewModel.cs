using PixChest.Composition.Bases;
using PixChest.Utils.Enums;
using PixChest.ViewModels.Files;
using PixChest.ViewModels.Thumbnails.FileTypes.Base;
using PixChest.ViewModels.Thumbnails.FileTypes.Image;
using PixChest.ViewModels.Thumbnails.FileTypes.Pdf;
using PixChest.ViewModels.Thumbnails.FileTypes.Video;

namespace PixChest.ViewModels.Thumbnails;
[AddTransient]
public class ThumbnailPickerSelectorViewModel: ViewModelBase {

	public ThumbnailPickerSelectorViewModel(
		ImageThumbnailPickerViewModel imageThumbnailPickerViewModel,
		PdfThumbnailPickerViewModel pdfThumbnailPickerViewModel,
		VideoThumbnailPickerViewModel videoThumbnailPickerViewModel) {
		this.ImageThumbnailPickerViewModel = imageThumbnailPickerViewModel;
		this.PdfThumbnailPickerViewModel = pdfThumbnailPickerViewModel;
		this.VideoThumbnailPickerViewModel = videoThumbnailPickerViewModel;

		this.FileViewModel.Subscribe(async x => {
			if(x == null) {
				this.ThumbnailPickerViewModel.Value = null;
				return;
			}
			this.ThumbnailPickerViewModel.Value = x.MediaType switch {
				MediaType.Video => this.VideoThumbnailPickerViewModel,
				MediaType.Pdf => this.PdfThumbnailPickerViewModel,
				_ => this.ImageThumbnailPickerViewModel
			};

			await this.ThumbnailPickerViewModel.Value.LoadAsync(x);
		});
	}

	public BindableReactiveProperty<IThumbnailPickerViewModel?> ThumbnailPickerViewModel {
		get;
	} = new();

	public ImageThumbnailPickerViewModel ImageThumbnailPickerViewModel {
		get;
	}

	public PdfThumbnailPickerViewModel PdfThumbnailPickerViewModel {
		get;
	}

	public VideoThumbnailPickerViewModel VideoThumbnailPickerViewModel {
		get;
	}

	public BindableReactiveProperty<FileViewModel> FileViewModel {
		get;
	} = new();
}
