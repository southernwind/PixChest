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

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		this.ViewModel.WindowActivatedCommand.Execute(Unit.Default);
	}
}
