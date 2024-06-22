
using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PixChest.ViewModels.FolderManager;
using PixChest.Views.Settings;

using Windows.Graphics;
using Windows.Storage.Pickers;

namespace PixChest.Views.FolderManager;
public sealed partial class FolderManagerWindow : Window {
	public FolderManagerWindowViewModel ViewModel {
		get;
	}
	public FolderManagerWindow(FolderManagerWindowViewModel viewModel) {
		this.InitializeComponent();
		this.ViewModel = viewModel;
		this.AppWindow.Resize(new SizeInt32(600,400));
	}

	private async void AddFolderButton_Click(object sender, RoutedEventArgs e) {
		var openPicker = new FolderPicker();

		var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
		WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
		openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
		openPicker.FileTypeFilter.Add("*");

		var folder = await openPicker.PickSingleFolderAsync();
		if (folder == null) {
			return;
		}
		this.ViewModel.AddFolderCommand.Execute(folder.Path);
	}
	private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
		if (sender is not MenuFlyoutItem selectedItem) {
			return;
		}
		switch (selectedItem.Tag.ToString()) {
			case "Remove":
				if(selectedItem.DataContext is not FolderViewModel target) {
					return;
				}

				var dialog = new ContentDialog {
					XamlRoot = this.Content.XamlRoot,
					Title = "Are you sure you want to delete this folder? This action cannot be undone.",
					PrimaryButtonText = "Remove",
					CloseButtonText = "Cancel",
					DefaultButton = ContentDialogButton.Primary
				};

				var result = await dialog.ShowAsync();

				if(result != ContentDialogResult.Primary) {
					return;
				}
				this.ViewModel.RemoveFolderCommand.Execute(target);
				break;
		}
	}

}
