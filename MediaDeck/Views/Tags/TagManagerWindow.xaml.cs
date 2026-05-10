using MediaDeck.Core.Stores.Config;
using MediaDeck.ViewModels.Tags;
using MediaDeck.Views.Helpers;

namespace MediaDeck.Views.Tags;

[Inject(InjectServiceLifetime.Transient)]
public sealed partial class TagManagerWindow : Microsoft.UI.Xaml.Window {
	private readonly CompositeDisposable _disposable = new();

	public TagManagerWindow(TagManagerViewModel viewModel, IConfigStore configStore) {
		this.ViewModel = viewModel;
		this.InitializeComponent();

		// テーマのバインド
		ThemeHelper.BindTheme(this, configStore, this._disposable);

		this.AppWindow.Resize(new(1000, 700));
		this.ViewModel.RequestClose.Subscribe(_ => {
			this.Close();
		}).AddTo(this._disposable);

		this.Closed += (s, e) => this._disposable.Dispose();
	}

	public TagManagerViewModel ViewModel {
		get;
	}

	private void Window_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e) {
		this.ExtendsContentIntoTitleBar = true;
		this.SetTitleBar(this.AppTitleBar);
	}
}