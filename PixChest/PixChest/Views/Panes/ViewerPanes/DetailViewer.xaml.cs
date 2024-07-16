using CommunityToolkit.WinUI;

using Microsoft.UI.Xaml.Controls;

namespace PixChest.Views.Panes.ViewerPanes;
public sealed partial class DetailViewer : ViewerPaneBase {
	public DetailViewer() {
		this.InitializeComponent();
	}

	protected override async void List_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		if (sender is ListView listView) {
			await listView.SmoothScrollIntoViewWithIndexAsync(listView.SelectedIndex, ScrollItemPlacement.Center, false, true);
		}
	}

}

