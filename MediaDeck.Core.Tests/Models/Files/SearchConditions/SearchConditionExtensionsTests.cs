using System.Linq.Expressions;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

/// <summary>
///     <see cref="SearchConditionExtensions"/> のテストクラスです。
/// </summary>
public class SearchConditionExtensionsTests {
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

	private class StubSearchCondition : ISearchCondition {
		public Expression<Func<MediaItem, bool>>? WherePredicate {
			get; set;
		}
		public string DisplayText {
			get {
				return "stub";
			}
		}

		public bool IsMatchForSuggest(string searchWord) {
			return false;
		}
	}

	/// <summary>
	///     条件が適用されてフィルタリングされることを確認します。
	/// </summary>
	[Fact]
	public void Where_AppliesConditions() {
		// Arrange
		var items = new[] {
			CreateMediaItem("a.jpg", rate: 1),
			CreateMediaItem("b.jpg", rate: 5),
			CreateMediaItem("c.jpg", rate: 3)
		}.AsQueryable();

		var conditions = new ISearchCondition[] {
			new StubSearchCondition { WherePredicate = mi => mi.Rate > 2 }
		};

		// Act
		var result = items.Where(conditions).ToList();

		// Assert
		result.Count.ShouldBe(2);
		result.ShouldAllBe(mi => mi.Rate > 2);
	}

	/// <summary>
	///     WherePredicateがnullの条件はスキップされることを確認します。
	/// </summary>
	[Fact]
	public void Where_SkipsNullPredicates() {
		// Arrange
		var items = new[] {
			CreateMediaItem("a.jpg", rate: 1),
			CreateMediaItem("b.jpg", rate: 5)
		}.AsQueryable();

		var conditions = new ISearchCondition[] {
			new StubSearchCondition { WherePredicate = null },
			new StubSearchCondition { WherePredicate = mi => mi.Rate > 2 }
		};

		// Act
		var result = items.Where(conditions).ToList();

		// Assert
		result.Count.ShouldBe(1);
	}

	/// <summary>
	///     空の条件リストでは全件返ることを確認します。
	/// </summary>
	[Fact]
	public void Where_EmptyConditions_ReturnsAll() {
		// Arrange
		var items = new[] {
			CreateMediaItem("a.jpg"),
			CreateMediaItem("b.jpg")
		}.AsQueryable();

		// Act
		var result = items.Where(Array.Empty<ISearchCondition>()).ToList();

		// Assert
		result.Count.ShouldBe(2);
	}
}