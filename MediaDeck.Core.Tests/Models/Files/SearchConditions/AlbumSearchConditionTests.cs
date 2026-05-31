using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

/// <summary>
///     <see cref="AlbumSearchCondition"/> のテストクラスです。
/// </summary>
public class AlbumSearchConditionTests {
	[Fact]
	public void DisplayText_WithoutSubAlbums_ShowsAlbumPath() {
		var condition = new AlbumSearchCondition { AlbumPath = "Travel/Japan", IncludeSubAlbums = false };
		condition.DisplayText.ShouldBe("Album=Travel/Japan");
	}

	[Fact]
	public void DisplayText_WithSubAlbums_ShowsFlag() {
		var condition = new AlbumSearchCondition { AlbumPath = "Travel", IncludeSubAlbums = true };
		condition.DisplayText.ShouldBe("Album=Travel&IncludeSubAlbums");
	}

	[Fact]
	public void AlbumPath_ThrowsWhenNotInitialized() {
		var condition = new AlbumSearchCondition();
		Should.Throw<InvalidOperationException>(() => _ = condition.AlbumPath);
	}

	[Fact]
	public void WherePredicate_WithoutSubAlbums_IsNotNull() {
		var condition = new AlbumSearchCondition { AlbumPath = "Test", IncludeSubAlbums = false };
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Fact]
	public void WherePredicate_WithSubAlbums_IsNotNull() {
		var condition = new AlbumSearchCondition { AlbumPath = "Test", IncludeSubAlbums = true };
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Theory]
	[InlineData("travel", true)]
	[InlineData("xyz", false)]
	public void IsMatchForSuggest_MatchesCorrectly(string searchWord, bool expected) {
		var condition = new AlbumSearchCondition { AlbumPath = "Travel/Japan" };
		condition.IsMatchForSuggest(searchWord).ShouldBe(expected);
	}
}