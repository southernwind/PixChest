using Microsoft.UI.Xaml;

using PixChest.ViewModels;

namespace PixChest.Views;

[AddSingleton]
public sealed partial class MainWindow : Window {
	private readonly MainWindowViewModel ViewModel;
	public MainWindow(MainWindowViewModel viewModel) {
		this.ViewModel = viewModel;
		this.InitializeComponent();
	}
}
