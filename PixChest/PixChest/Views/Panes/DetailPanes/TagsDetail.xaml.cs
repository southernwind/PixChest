using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

using PixChest.Views.Tags;
using PixChest.Database.Tables;

namespace PixChest.Views.Panes.DetailPanes;
public sealed partial class TagsDetail : DetailPaneBase {
	public TagsDetail(){
		this.InitializeComponent();
	}

	private void OpenTagManagerButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
		var window = Ioc.Default.GetRequiredService<TagManagerWindow>();
		window.Activate();
	}

	private void AutoSuggestBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e) {
		if (e.Key == Windows.System.VirtualKey.Enter) {
			this.ViewModel?.AddTagCommand.Execute(Unit.Default);
		}

	}

	private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
		if (args.SelectedItem is not Tag tag) {
			return;
		}
		sender.Text = tag.TagName;
	}
}
