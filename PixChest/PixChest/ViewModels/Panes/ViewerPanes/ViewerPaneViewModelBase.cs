using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.ViewModels.Interfaces;
using PixChest.Models.FileDetailManagers;

namespace PixChest.ViewModels.Panes.ViewerPanes;

public abstract class ViewerPaneViewModelBase(string name, FilesManager filesManager) : ViewModelBase {
	public string Name {
		get;
	} = name;

	public async Task RemoveFileAsync(IFileViewModel fileViewModel) {
		await filesManager.RemoveFileAsync(fileViewModel.FileModel);
	}
}
