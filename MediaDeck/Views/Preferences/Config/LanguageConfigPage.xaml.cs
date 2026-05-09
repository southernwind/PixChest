using MediaDeck.ViewModels.Preferences.Config;

using Microsoft.UI.Xaml.Navigation;

namespace MediaDeck.Views.Preferences.Config;

public sealed partial class LanguageConfigPage {
	public LanguageConfigPage() {
		this.InitializeComponent();
	}

	/// <summary>
	/// ナビゲート時に ViewModel を受け取ります。
	/// </summary>
	protected override void OnNavigatedTo(NavigationEventArgs e) {
		if (e.Parameter is not LanguageConfigPageViewModel vm) {
			throw new InvalidOperationException("ViewModel is not passed.");
		}
		this.ViewModel = vm;

		// ComboBox のバインディングをコードで設定
		this.LanguageComboBox.ItemsSource = vm.LanguageOptions;
		this.LanguageComboBox.SelectedItem = vm.SelectedLanguage.Value;
		this.LanguageComboBox.SelectionChanged += (s, _) => {
			if (this.LanguageComboBox.SelectedItem is LanguageOption opt) {
				vm.SelectedLanguage.Value = opt;
			}
		};

		base.OnNavigatedTo(e);
	}

	/// <summary>
	/// ビューモデル
	/// </summary>
	public LanguageConfigPageViewModel? ViewModel {
		get;
		set;
	}
}