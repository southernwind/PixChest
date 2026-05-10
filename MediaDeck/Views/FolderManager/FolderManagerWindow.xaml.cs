using MediaDeck.Core.Stores.Config;
using MediaDeck.ViewModels.FolderManager;
using MediaDeck.Views.Helpers;

namespace MediaDeck.Views.FolderManager;

[Inject(InjectServiceLifetime.Transient)]
public sealed partial class FolderManagerWindow : Microsoft.UI.Xaml.Window {
	private readonly CompositeDisposable _disposable = new();

	public FolderManagerWindow(FolderManagerViewModel viewModel, IConfigStore configStore) {
		this.ViewModel = viewModel;
		this.InitializeComponent();

		// テーマのバインド
		ThemeHelper.BindTheme(this, configStore, this._disposable);

		this.AppWindow.Resize(new(1000, 700));

		this.Closed += (s, e) => this._disposable.Dispose();
	}

	public FolderManagerViewModel ViewModel {
		get;
	}

	private void Window_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(this.AppTitleBar);
	}
}