using Microsoft.UI.Xaml;

using PixChest.ViewModels.Thumbnails;

using Windows.Graphics;

namespace PixChest.Views.Thumbnails;
[AddTransient]
public sealed partial class ThumbnailPickerWindow : Window {
	public ThumbnailPickerWindow(ThumbnailPickerSelectorViewModel thumbnailPickerSelectorViewModel) {
		this.InitializeComponent();
		this.ViewModel = thumbnailPickerSelectorViewModel;
		this.AppWindow.Resize(new SizeInt32(1000, 700));
	}

	public ThumbnailPickerSelectorViewModel ViewModel {
		get;
	}
}
