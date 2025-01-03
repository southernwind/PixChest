using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.WinUI.Controls;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Models.Files.SearchConditions;
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


	protected async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e) {
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
			case "RemoveFile":
				await DialogUtility.ConfirmDialogAndAction(
					this.XamlRoot,
					async () => await this.ViewModel.SelectedViewerPane.Value.RemoveFileAsync(fvm),
					"Remove file from PixChest database?",
					_ => "File removed from PixChest database"
					);

				break;
		}
	}

	protected void TokenizingTextBox_TokenItemAdding(TokenizingTextBox sender, TokenItemAddingEventArgs args) {
		args.Cancel = true;
		this.ViewModel?.MediaContentLibraryViewModel.SearchConditionNotificationDispatcher.AddRequest.OnNext(new WordSearchCondition(args.TokenText));
	}

	protected void TokenizingTextBox_TokenItemRemoving(TokenizingTextBox sender, TokenItemRemovingEventArgs args) {
		args.Cancel = true;
		if (args.Item is not SearchConditionViewModel { } condition) {
			return;
		}
		this.ViewModel?.MediaContentLibraryViewModel.SearchConditionNotificationDispatcher.RemoveRequest.OnNext(condition.SearchCondition);
	}
	protected void TokenizingTextBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
		if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
			this.ViewModel?.MediaContentLibraryViewModel.RefreshSearchTokenCandidates(sender.Text);
		}
	}
}