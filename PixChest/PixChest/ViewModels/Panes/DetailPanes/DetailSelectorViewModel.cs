using PixChest.Composition.Bases;
using PixChest.ViewModels.Files;

namespace PixChest.ViewModels.Panes.DetailPanes;

[AddTransient]
public class DetailSelectorViewModel:ViewModelBase
{
	public BindableReactiveProperty<FileViewModel> TargetFile {
		get;
	} = new();
}
