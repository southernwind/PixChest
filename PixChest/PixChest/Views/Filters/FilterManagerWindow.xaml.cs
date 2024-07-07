using Microsoft.UI.Xaml;

using PixChest.ViewModels.Filters;

using Windows.Graphics;
namespace PixChest.Views.Filters;
[AddTransient]
public sealed partial class FilterManagerWindow : Window {
	public FilterManagerWindow(FilterManagerViewModel filterManagerViewModel) {
		this.InitializeComponent();
		this.ViewModel = filterManagerViewModel;
		this.ViewModel.LoadCommand.Execute(Unit.Default);
		this.AppWindow.Resize(new SizeInt32(600, 400));
	}

	public FilterManagerViewModel ViewModel {
		get;
	}
}
