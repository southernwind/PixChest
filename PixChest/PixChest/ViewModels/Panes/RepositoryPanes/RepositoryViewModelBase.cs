using PixChest.Models.Repositories;

namespace PixChest.ViewModels.Panes.RepositoryPanes;

public class RepositoryViewModelBase(RepositoryBase model) {
	public RepositoryBase Model {
		get;
	} = model;
}
