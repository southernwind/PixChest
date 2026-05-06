using System.Runtime.CompilerServices;
using MediaDeck.Core.Models.Repositories;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Repositories;

/// <summary>
/// <see cref="RepositorySelector"/> のテストクラスです。
/// </summary>
public class RepositorySelectorTest {
	/// <summary>
	/// コンストラクタがプロパティを正しく初期化することを確認します。
	/// </summary>
	[Fact]
	public void Constructor_InitializesPropertiesCorrectly() {
		// Arrange
		// FolderRepositoryは依存関係が多くコンストラクタが重いため、RuntimeHelpersを使用して未初期化インスタンスを生成します。
		// RepositorySelectorはインスタンスを保持するだけなので、これで十分です。
		var dummyFolderRepository = RuntimeHelpers.GetUninitializedObject(typeof(FolderRepository)) as FolderRepository;
		var dummyAlbumRepository = RuntimeHelpers.GetUninitializedObject(typeof(AlbumRepository)) as AlbumRepository;

		// Act
		var selector = new RepositorySelector(dummyFolderRepository!, dummyAlbumRepository!);

		// Assert
		selector.Repositories.ShouldNotBeNull();
		selector.Repositories.Length.ShouldBe(2);
		selector.Repositories[0].ShouldBeSameAs(dummyFolderRepository);
		selector.Repositories[1].ShouldBeSameAs(dummyAlbumRepository);

		selector.SelectedRepository.ShouldNotBeNull();
		selector.SelectedRepository.Value.ShouldBeSameAs(dummyFolderRepository);
	}
}