using System.ComponentModel;

using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Files.Sort;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.Sort;

/// <summary>
///     <see cref="SortItem"/> のテストクラスです。
/// </summary>
public class SortItemTests {
	private static MediaItem CreateMediaItem(string filePath, int rate = 0) {
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
	///     Ascending方向でApplySortが昇順にソートすることを確認します。
	/// </summary>
	[Fact]
	public void ApplySort_Ascending_SortsCorrectly() {
		// Arrange
		var sortItem = new SortItem(SortItemKey.FilePath, mf => (object?)mf.FilePath, ListSortDirection.Ascending);
		var items = new[] {
			CreateMediaItem("c.jpg"),
			CreateMediaItem("a.jpg"),
			CreateMediaItem("b.jpg")
		}.AsQueryable();

		// Act
		var result = sortItem.ApplySort(items, false).ToList();

		// Assert
		result[0].FilePath.ShouldBe("a.jpg");
		result[1].FilePath.ShouldBe("b.jpg");
		result[2].FilePath.ShouldBe("c.jpg");
	}

	/// <summary>
	///     Descending方向でApplySortが降順にソートすることを確認します。
	/// </summary>
	[Fact]
	public void ApplySort_Descending_SortsCorrectly() {
		// Arrange
		var sortItem = new SortItem(SortItemKey.FilePath, mf => (object?)mf.FilePath, ListSortDirection.Descending);
		var items = new[] {
			CreateMediaItem("a.jpg"),
			CreateMediaItem("c.jpg"),
			CreateMediaItem("b.jpg")
		}.AsQueryable();

		// Act
		var result = sortItem.ApplySort(items, false).ToList();

		// Assert
		result[0].FilePath.ShouldBe("c.jpg");
		result[1].FilePath.ShouldBe("b.jpg");
		result[2].FilePath.ShouldBe("a.jpg");
	}

	/// <summary>
	///     reverseフラグがtrueの場合、ソート方向が反転することを確認します。
	/// </summary>
	[Fact]
	public void ApplySort_WithReverse_InvertsDirection() {
		// Arrange
		var sortItem = new SortItem(SortItemKey.FilePath, mf => (object?)mf.FilePath, ListSortDirection.Ascending);
		var items = new[] {
			CreateMediaItem("a.jpg"),
			CreateMediaItem("c.jpg"),
			CreateMediaItem("b.jpg")
		}.AsQueryable();

		// Act
		var result = sortItem.ApplySort(items, true).ToList();

		// Assert
		result[0].FilePath.ShouldBe("c.jpg");
		result[1].FilePath.ShouldBe("b.jpg");
		result[2].FilePath.ShouldBe("a.jpg");
	}

	/// <summary>
	///     ApplyThenBySortが二次ソートとして正しく機能することを確認します。
	/// </summary>
	[Fact]
	public void ApplyThenBySort_SortsAsSecondaryKey() {
		// Arrange
		var primarySort = new SortItem(SortItemKey.Rate, mf => (object?)mf.Rate, ListSortDirection.Ascending);
		var secondarySort = new SortItem(SortItemKey.FilePath, mf => (object?)mf.FilePath, ListSortDirection.Ascending);
		var items = new[] {
			CreateMediaItem("b.jpg", rate: 1),
			CreateMediaItem("a.jpg", rate: 1),
			CreateMediaItem("c.jpg", rate: 0)
		}.AsQueryable();

		// Act
		var ordered = primarySort.ApplySort(items, false);
		var result = secondarySort.ApplyThenBySort(ordered, false).ToList();

		// Assert
		result[0].FilePath.ShouldBe("c.jpg"); // rate 0
		result[1].FilePath.ShouldBe("a.jpg"); // rate 1, a < b
		result[2].FilePath.ShouldBe("b.jpg"); // rate 1
	}

	/// <summary>
	///     ToStringが例外をスローしないことを確認します。
	/// </summary>
	[Fact]
	public void ToString_ContainsKey() {
		// Arrange
		var sortItem = new SortItem(SortItemKey.Rate, mf => (object?)mf.Rate);

		// Act
		var result = sortItem.ToString();

		// Assert
		result.ShouldContain("Rate");
	}
}