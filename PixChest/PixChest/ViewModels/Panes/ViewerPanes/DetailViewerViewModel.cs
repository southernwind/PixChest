using System;
using System.IO;

using PixChest.Composition.Bases;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.ViewerPanes;
public class DetailViewerViewModel :ViewModelBase {
	public ReactiveCollection<FileViewModel> Files {
		get;
	} = new();

	public ReactiveCommand ReloadCommand {
		get;
	} = new();

	public DetailViewerViewModel() {
		this.ReloadCommand.Subscribe(_ => {
			var files = Directory.EnumerateFiles(@"C:\Users\admin\Pictures", "", SearchOption.AllDirectories);
			this.Files.Clear();
			this.Files.AddRangeOnScheduler(files.Select(x => {
				var vm = new FileViewModel();
				vm.FilePath.Value = x;
				return vm;
			}));
		});
	}
}
