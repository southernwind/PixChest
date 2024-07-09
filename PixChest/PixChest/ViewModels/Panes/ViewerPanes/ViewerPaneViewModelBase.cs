using System.Diagnostics;
using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;

public abstract class ViewerPaneViewModelBase : ViewModelBase {
	public ViewerPaneViewModelBase(MediaContentLibrary mediaContentLibrary, string name) {
		this.Files = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(mediaContentLibrary.Files, x => new FileViewModel(x));
		this.ExecuteCommand.Subscribe(async _ => {
			await this.Execute();
		}).AddTo(this.CompositeDisposable);
		this.Name = name;
	}

	public string Name {
		get;
	}

	public Reactive.Bindings.ReadOnlyReactiveCollection<FileViewModel> Files {
		get;
	}

	public BindableReactiveProperty<FileViewModel> SelectedFile {
		get;
	} = new();

	public ReactiveCommand<Unit> ExecuteCommand {
		get;
	} = new();

	public virtual Task Execute() {
		var psi = new ProcessStartInfo {
			FileName = this.SelectedFile.Value.FilePath,
			UseShellExecute = true
		};
		_ = Process.Start(psi);
		return Task.CompletedTask;
	}
}
