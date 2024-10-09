using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PixChest.Utils.Objects;
using PixChest.Views.Tags;

namespace PixChest.Views.Panes.DetailPanes;
public sealed partial class TagsDetail : DetailPaneBase {
	public TagsDetail() {
		this.InitializeComponent();
	}

	private void AutoSuggestBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e) {
		if (e.Key == Windows.System.VirtualKey.Enter) {
			this.ViewModel?.AddTagCommand.Execute(Unit.Default);
		}
	}

	private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
		if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
			this.ViewModel?.RefreshFilteredTagCandidatesCommand.Execute(Unit.Default);
		}
	}

	private void TagGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
		if (this.TagListBox.SelectedItem is not ValueCountPair<string> tag) {
			return;
		}
		this.ViewModel?.SearchTaggedFilesCommand.Execute(tag);
    }
}