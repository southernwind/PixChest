using System.Threading.Tasks;

namespace PixChest.Models.Repositories;

public abstract class RepositoryBase {
	public abstract Task Load();
}
