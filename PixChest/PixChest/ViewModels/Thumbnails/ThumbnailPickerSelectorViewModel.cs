using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.FileTypes.Image.ViewModels;
using PixChest.FileTypes.Pdf.ViewModels;
using PixChest.FileTypes.Video.ViewModels;
using PixChest.Utils.Enums;
using PixChest.ViewModels.Files;

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
