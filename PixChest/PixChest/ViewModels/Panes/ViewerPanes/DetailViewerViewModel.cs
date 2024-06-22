using System.Collections.Generic;

using PixChest.Composition.Bases;
using PixChest.Models.Files;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;
public class DetailViewerViewModel :ViewModelBase {
	public IReadOnlyCollection<FileViewModel> Files {
		get;
	}

	public AsyncReactiveCommand ReloadCommand {
		get;
	} = new();

	public DetailViewerViewModel(MediaContentLibrary mediaContentLibrary) {
		this.Files = mediaContentLibrary.Files.ToReadOnlyReactiveCollection(x => new FileViewModel(x));
		this.ReloadCommand.Subscribe(async _ => {
			await mediaContentLibrary.Search();
		});
	}
}
