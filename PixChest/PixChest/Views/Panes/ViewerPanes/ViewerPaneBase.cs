using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.ViewModels.Panes.ViewerPanes;
using PixChest.Views.Thumbnails;

namespace PixChest.Views.Panes.ViewerPanes;

public class ViewerPaneBase : UserControlBase<ViewerSelectorViewModel> {
	protected virtual void List_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (this.ViewModel is not { } vm) {
			return;
		}
		if (sender is ListBox listBox) {
			vm.MediaContentLibraryViewModel.SelectedFiles.Value = listBox.SelectedItems.Select(x => x as IFileViewModel).Where(x => x is not null).ToArray()!;
		} else if (sender is GridView gridView) {
			vm.MediaContentLibraryViewModel.SelectedFiles.Value = gridView.SelectedItems.Select(x => x as IFileViewModel).Where(x => x is not null).ToArray()!;
		}
	}

	protected async void File_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
		if (sender is not Grid grid) {
			return;
		}
		if (grid.DataContext is not IFileViewModel fileViewModel) {
			return;
		}
		await fileViewModel.ExecuteFileAsync();
	}


	protected void MenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
		if (this.ViewModel is null) {
			return;
		}
		if (sender is not MenuFlyoutItem selectedItem) {
			return;
		}
		if (selectedItem.DataContext is not IFileViewModel fvm) {
			return;
		}
		switch (selectedItem.Tag.ToString()) {
			case "RecreateThumbnail":
				var window = Ioc.Default.GetRequiredService<ThumbnailPickerWindow>();
				window.ViewModel.FileViewModel.Value = fvm;
				window?.Activate();
				break;
		}
	}

	protected void TokenizingTextBox_TokenItemAdding(CommunityToolkit.WinUI.Controls.TokenizingTextBox sender, CommunityToolkit.WinUI.Controls.TokenItemAddingEventArgs args) {
		args.Cancel = true;
		this.ViewModel?.MediaContentLibraryViewModel.AddWordSearchCondition(args.TokenText);
	}

	protected void TokenizingTextBox_TokenItemRemoving(CommunityToolkit.WinUI.Controls.TokenizingTextBox sender, CommunityToolkit.WinUI.Controls.TokenItemRemovingEventArgs args) {
		args.Cancel = true;
		if (args.Item is not SearchConditionViewModel { } condition) {
			return;
		}
		this.ViewModel?.MediaContentLibraryViewModel.RemoveSearchCondition(condition);
	}
}