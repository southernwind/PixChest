using MediaDeck.Core.Models.Repositories.Objects;
using MediaDeck.Core.Primitives;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Repositories.Objects;

/// <summary>
///     <see cref="AlbumObject"/> のテストクラスです。
/// </summary>
public class AlbumObjectTests {
	/// <summary>
	///     空のアルバムリストでルートノードが正しく構築されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_EmptyList_CreatesRootWithNoChildren() {
		// Arrange & Act
		var root = new AlbumObject(null, "", []);

		// Assert
		root.AlbumName.ShouldBe("Albums");
		root.AlbumPath.ShouldBe("");
		root.ChildAlbums.ShouldBeEmpty();
		root.FileCount.ShouldBe(0);
		root.Parent.ShouldBeNull();
	}

	/// <summary>
	///     単一のアルバムでルートの子ノードが1つ作られることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_SingleAlbum_CreatesOneChild() {
		// Arrange
		var albums = new[] {
			new ValueCountPair<string>("", 5),
			new ValueCountPair<string>("Vacation", 5)
		};

		// Act
		var root = new AlbumObject(null, "", albums);

		// Assert
		root.ChildAlbums.Length.ShouldBe(1);
		root.ChildAlbums[0].AlbumPath.ShouldBe("Vacation");
		root.ChildAlbums[0].AlbumName.ShouldBe("Vacation");
	}

	/// <summary>
	///     ネストしたアルバムで階層が正しく構築されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_NestedAlbums_CreatesHierarchy() {
		// Arrange
		var albums = new[] {
			new ValueCountPair<string>("", 8),
			new ValueCountPair<string>("Travel", 3),
			new ValueCountPair<string>("Travel/Japan", 5)
		};

		// Act
		var root = new AlbumObject(null, "", albums);

		// Assert
		root.ChildAlbums.Length.ShouldBe(1);
		var travel = root.ChildAlbums[0];
		travel.AlbumPath.ShouldBe("Travel");
		travel.ChildAlbums.Length.ShouldBe(1);
		travel.ChildAlbums[0].AlbumPath.ShouldBe("Travel/Japan");
		travel.ChildAlbums[0].AlbumName.ShouldBe("Japan");
	}

	/// <summary>
	///     FileCountがすべての子孫を合算した値であることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_FileCount_SumsAllDescendants() {
		// Arrange
		var albums = new[] {
			new ValueCountPair<string>("A", 3),
			new ValueCountPair<string>("A/B", 5)
		};

		// Act
		var root = new AlbumObject(null, "", albums);

		// Assert
		root.FileCount.ShouldBe(8);
	}

	/// <summary>
	///     Separatorが'/'であることを確認します。
	/// </summary>
	[Fact]
	public void Separator_IsForwardSlash() {
		AlbumObject.Separator.ShouldBe('/');
	}

	/// <summary>
	///     IsExpandedのデフォルト値がfalseであることを確認します。
	/// </summary>
	[Fact]
	public void IsExpanded_DefaultIsFalse() {
		// Arrange & Act
		var album = new AlbumObject(null, "", []);

		// Assert
		album.IsExpanded.ShouldBeFalse();
	}
}