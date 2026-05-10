using MediaDeck.Core.Stores.Config;
using MediaDeck.ViewModels.Tags;
using MediaDeck.Views.Helpers;

namespace MediaDeck.Views.Tags;

[Inject(InjectServiceLifetime.Transient)]
public sealed partial class NewTagDialog {
	private readonly CompositeDisposable _disposable = new();

	public NewTagDialog(NewTagDialogViewModel viewModel, IConfigStore configStore) {
		this.ViewModel = viewModel;
		this.InitializeComponent();

		ThemeHelper.BindTheme(this, configStore, this._disposable);
		this.Closed += (_, _) => this._disposable.Dispose();

		this.PrimaryButtonClick += (_, _) => {
			this.ViewModel.ConfirmCommand.Execute(Unit.Default);
		};

		this.CloseButtonClick += (_, _) => {
			this.ViewModel.CancelCommand.Execute(Unit.Default);
		};
	}

	public NewTagDialogViewModel ViewModel {
		get;
	}
}