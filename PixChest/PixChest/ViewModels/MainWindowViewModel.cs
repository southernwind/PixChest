using R3;

namespace PixChest.ViewModels;
public class MainWindowViewModel {
	public ReactiveProperty<string> Text {
		get;
	} = new();

	public MainWindowViewModel() {
		this.Text.Value = "Hello, World!";
	}
}
