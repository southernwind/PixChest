using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using PixChest.Composition.Bases;
using PixChest.ViewModels.Panes.FilterPanes;
using PixChest.Views.Filters;

namespace PixChest.Views.Panes.FilterPanes;
public sealed partial class FilterSelector : FilterSelectorUserControl {
	public FilterSelector() {
		this.InitializeComponent();
	}

	private void OpenFilterSettingsWindowButton_Click(object sender, RoutedEventArgs e) {
		var window = Ioc.Default.GetRequiredService<FilterManagerWindow>();
		window.Activate();
	}
}
public abstract class FilterSelectorUserControl : UserControlBase<FilterSelectorViewModel>;

