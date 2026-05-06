namespace MediaDeck.Core.Models.Repositories;

[Inject(InjectServiceLifetime.Scoped)]
public class RepositorySelector {
	public RepositorySelector(FolderRepository folderRepository, AlbumRepository albumRepository) {
		this.Repositories = [
			folderRepository,
			albumRepository
		];
		this.SelectedRepository.Value = folderRepository;
	}

	public RepositoryBase[] Repositories {
		get;
	}

	public ReactiveProperty<RepositoryBase> SelectedRepository {
		get;
	} = new();
}