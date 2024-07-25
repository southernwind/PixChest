using PixChest.Models.Repositories.Objects;
using PixChest.Models.Settings;

namespace PixChest.Models.Repositories;
[AddSingleton]
public class RepositorySelector {

	public RepositorySelector(FolderRepository folderRepository,States states) {
		this.Repositories = [
			folderRepository
		];
		this.SelectedRepository.Value = folderRepository;
		this.CurrentRepositoryCondition = states.SearchStates.CurrentRepositoryCondition.ToReadOnlyReactiveProperty();
	}
	public RepositoryBase[] Repositories {
		get;
	}

	public ReactiveProperty<RepositoryBase> SelectedRepository {
		get;
	} = new();

	/// <summary>
	/// カレントリポジトリ条件
	/// </summary>
	public ReadOnlyReactiveProperty<RepositoryConditionObject?> CurrentRepositoryCondition {
		get;
	}
}
