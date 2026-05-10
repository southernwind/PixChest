using MediaDeck.ViewModels.Preferences.Config;

using Microsoft.UI.Xaml.Navigation;

namespace MediaDeck.Views.Preferences.Config;

public sealed partial class EnvironmentConfigPage {
	public EnvironmentConfigPage() {
		this.InitializeComponent();
	}

	/// <summary>
	/// ナビゲート時に ViewModel を受け取ります。
	/// </summary>
	protected override void OnNavigatedTo(NavigationEventArgs e) {
		if (e.Parameter is not EnvironmentConfigPageViewModel vm) {
			throw new InvalidOperationException("ViewModel is not passed.");
		}
		this.ViewModel = vm;
		base.OnNavigatedTo(e);
	}

	/// <summary>
	/// ビューモデル
	/// </summary>
	public EnvironmentConfigPageViewModel? ViewModel {
		get;
		set;
	}
}