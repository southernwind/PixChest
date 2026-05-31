using MediaDeck.Core.Models.Repositories.Objects;
using MediaDeck.Core.Primitives;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Repositories.Objects;

/// <summary>
///     <see cref="FolderObject"/> のテストクラスです。
/// </summary>
public class FolderObjectTests {
	private static readonly char Sep = Path.DirectorySeparatorChar;

	/// <summary>
	///     空のディレクトリリストでルートノードが正しく構築されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_EmptyList_CreatesRootWithNoChildren() {
		// Arrange & Act
		var root = new FolderObject(null, "", []);

		// Assert
		root.FolderName.ShouldBe("PC");
		root.FolderPath.ShouldBe("");
		root.ChildFolders.ShouldBeEmpty();
		root.FileCount.ShouldBe(0);
		root.Parent.ShouldBeNull();
	}

	/// <summary>
	///     単一のディレクトリでルートの子ノードが1つ作られることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_SingleDirectory_CreatesOneChild() {
		// Arrange
		var path = $"C:{Sep}Photos";
		var dirs = new[] { new ValueCountPair<string>("", 5), new ValueCountPair<string>(path, 5) };

		// Act
		var root = new FolderObject(null, "", dirs);

		// Assert
		root.ChildFolders.Length.ShouldBe(1);
		root.ChildFolders[0].FolderPath.ShouldBe(path);
		root.ChildFolders[0].FolderName.ShouldBe(path);
		root.FileCount.ShouldBe(10);
	}

	/// <summary>
	///     ネストしたディレクトリで階層が正しく構築されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_NestedDirectories_CreatesHierarchy() {
		// Arrange
		var parentPath = $"C:{Sep}Photos";
		var childPath = $"C:{Sep}Photos{Sep}2024";
		var dirs = new[] {
			new ValueCountPair<string>("", 8),
			new ValueCountPair<string>(parentPath, 3),
			new ValueCountPair<string>(childPath, 5)
		};

		// Act
		var root = new FolderObject(null, "", dirs);

		// Assert
		root.ChildFolders.Length.ShouldBe(1);
		var parent = root.ChildFolders[0];
		parent.FolderPath.ShouldBe(parentPath);
		parent.ChildFolders.Length.ShouldBe(1);
		parent.ChildFolders[0].FolderPath.ShouldBe(childPath);
	}

	/// <summary>
	///     子ノードのFolderNameが親パスを除去した名前になることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_ChildFolderName_ExcludesParentPath() {
		// Arrange
		var parentPath = $"C:{Sep}Photos";
		var childPath = $"C:{Sep}Photos{Sep}Summer";
		var dirs = new[] {
			new ValueCountPair<string>(parentPath, 2),
			new ValueCountPair<string>(childPath, 3)
		};

		// Act
		var parent = new FolderObject(null, "", dirs);
		var child = parent.ChildFolders[0]; // C:\Photos
		var grandchild = child.ChildFolders[0]; // C:\Photos\Summer

		// Assert
		grandchild.FolderName.ShouldBe("Summer");
	}

	/// <summary>
	///     IsExpandedのデフォルト値がfalseであることを確認します。
	/// </summary>
	[Fact]
	public void IsExpanded_DefaultIsFalse() {
		// Arrange & Act
		var folder = new FolderObject(null, "", []);

		// Assert
		folder.IsExpanded.ShouldBeFalse();
	}
}