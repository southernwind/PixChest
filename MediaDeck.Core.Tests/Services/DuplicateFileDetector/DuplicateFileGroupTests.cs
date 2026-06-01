using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Services.DuplicateFileDetector;
using Shouldly;

namespace MediaDeck.Core.Tests.Services.DuplicateFileDetector;

/// <summary>
///     <see cref="DuplicateFileGroup"/> のテストクラスです。
/// </summary>
public class DuplicateFileGroupTests {
	private static MediaItem CreateMediaItem(string filePath) {
		return new MediaItem {
			MediaType = MediaType.Image,
			DirectoryPath = "/test",
			FilePath = filePath,
			Description = "",
			IsUnderFolderGroup = false
		};
	}

	/// <summary>
	///     ファイルが存在する場合、RepresentativeFileNameが最初のファイルパスを返すことを確認します。
	/// </summary>
	[Fact]
	public void RepresentativeFileName_WithFiles_ReturnsFirstFilePath() {
		// Arrange
		var group = new DuplicateFileGroup {
			Hash = "abc123",
			Files = [CreateMediaItem("photo1.jpg"), CreateMediaItem("photo2.jpg")]
		};

		// Act & Assert
		group.RepresentativeFileName.ShouldBe("photo1.jpg");
	}

	/// <summary>
	///     ファイルが空の場合、RepresentativeFileNameがハッシュ値を返すことを確認します。
	/// </summary>
	[Fact]
	public void RepresentativeFileName_EmptyFiles_ReturnsHash() {
		// Arrange
		var group = new DuplicateFileGroup {
			Hash = "abc123",
			Files = []
		};

		// Act & Assert
		group.RepresentativeFileName.ShouldBe("abc123");
	}

	/// <summary>
	///     Hashプロパティが正しく設定されることを確認します。
	/// </summary>
	[Fact]
	public void Hash_IsSetCorrectly() {
		// Arrange
		var group = new DuplicateFileGroup {
			Hash = "def456",
			Files = []
		};

		// Act & Assert
		group.Hash.ShouldBe("def456");
	}
}