using MediaDeck.Core.Stores.Config;
using MediaDeck.Views.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MediaDeck.Views.Dialogs;

[Inject(InjectServiceLifetime.Transient)]
public sealed partial class NewAlbumDialog : ContentDialog {
	private readonly CompositeDisposable _disposable = new();

	public NewAlbumDialog(IConfigStore configStore) {
		this.InitializeComponent();
		ThemeHelper.BindTheme(this, configStore, this._disposable);
		this.Closed += (_, _) => this._disposable.Dispose();
	}

	public string AlbumName {
		get;
		set;
	} = string.Empty;

	public string AlbumPath {
		get;
		set;
	} = string.Empty;

	public string FullAlbumPath {
		get {
			var name = (this.AlbumName ?? string.Empty).Trim();
			var path = (this.AlbumPath ?? string.Empty).Trim().Trim('/').Trim('\\');
			return string.IsNullOrWhiteSpace(path) ? name : $"{path}/{name}";
		}
	}

	private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
		if (!string.IsNullOrWhiteSpace(this.AlbumName)) {
			return;
		}
		args.Cancel = true;
	}

	private void AlbumNameTextBox_Loaded(object sender, RoutedEventArgs e) {
		this.AlbumNameTextBox.Focus(FocusState.Programmatic);
	}
}