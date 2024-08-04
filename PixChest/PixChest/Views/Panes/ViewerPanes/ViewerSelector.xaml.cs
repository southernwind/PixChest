using CommunityToolkit.Mvvm.DependencyInjection;

using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.ViewerPanes;
using PixChest.Views.Sort;

namespace PixChest.Views.Panes.ViewerPanes;
public sealed partial class ViewerSelector : ViewerSelectorUserControl {
	public ViewerSelector() {
		this.InitializeComponent();
	}

	private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
		var window = Ioc.Default.GetRequiredService<SortManagerWindow>();
		window.Activate();
	}
}
public abstract class ViewerSelectorUserControl: UserControlBase<ViewerSelectorViewModel>;
