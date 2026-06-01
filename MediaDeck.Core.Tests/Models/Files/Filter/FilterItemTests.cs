using System.Linq.Expressions;

using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Files.Filter;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.Filter;

/// <summary>
///     <see cref="FilterItem"/> のテストクラスです。
/// </summary>
public class FilterItemTests {
	private static MediaItem CreateMediaItem(string filePath = "test.jpg", int rate = 0) {
		return new MediaItem {
			MediaType = MediaType.Image,
			DirectoryPath = "/test",
			FilePath = filePath,
			Description = "",
			IsUnderFolderGroup = false,
			Rate = rate
		};
	}

	/// <summary>
	///     コンストラクタで渡した条件式がConditionプロパティに設定されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_SetsConditionProperty() {
		// Arrange
		Expression<Func<MediaItem, bool>> condition = mf => mf.Rate > 3;

		// Act
		var filterItem = new FilterItem(condition);

		// Assert
		filterItem.Condition.ShouldBe(condition);
	}

	/// <summary>
	///     Conditionをコンパイルして正しくフィルタリングできることを確認します。
	/// </summary>
	[Fact]
	public void Condition_CompiledPredicate_FiltersCorrectly() {
		// Arrange
		var filterItem = new FilterItem(mf => mf.Rate > 3);
		var predicate = filterItem.Condition.Compile();

		// Act & Assert
		predicate(CreateMediaItem(rate: 5)).ShouldBeTrue();
		predicate(CreateMediaItem(rate: 2)).ShouldBeFalse();
	}

	/// <summary>
	///     ToStringが例外をスローしないことを確認します。
	/// </summary>
	[Fact]
	public void ToString_DoesNotThrow() {
		// Arrange
		var filterItem = new FilterItem(mf => mf.Rate > 0);

		// Act
		var result = filterItem.ToString();

		// Assert
		result.ShouldNotBeNullOrWhiteSpace();
	}
}