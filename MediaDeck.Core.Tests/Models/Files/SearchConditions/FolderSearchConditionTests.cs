using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

/// <summary>
///     <see cref="FolderSearchCondition"/> のテストクラスです。
/// </summary>
public class FolderSearchConditionTests {
	[Fact]
	public void DisplayText_WithoutSubDirectories_ShowsFolderPath() {
		var condition = new FolderSearchCondition { FolderPath = "/test/dir", IncludeSubDirectories = false };
		condition.DisplayText.ShouldBe("Folder=/test/dir");
	}

	[Fact]
	public void DisplayText_WithSubDirectories_ShowsFlag() {
		var condition = new FolderSearchCondition { FolderPath = "/test/dir", IncludeSubDirectories = true };
		condition.DisplayText.ShouldBe("Folder=/test/dir&IncludeSubFolders");
	}

	[Fact]
	public void FolderPath_ThrowsWhenNotInitialized() {
		var condition = new FolderSearchCondition();
		Should.Throw<InvalidOperationException>(() => _ = condition.FolderPath);
	}

	[Fact]
	public void WherePredicate_WithoutSubDirectories_IsNotNull() {
		var condition = new FolderSearchCondition { FolderPath = "/test", IncludeSubDirectories = false };
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Fact]
	public void WherePredicate_WithSubDirectories_IsNotNull() {
		var condition = new FolderSearchCondition { FolderPath = "/test", IncludeSubDirectories = true };
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Theory]
	[InlineData("test", true)]
	[InlineData("xyz", false)]
	public void IsMatchForSuggest_MatchesCorrectly(string searchWord, bool expected) {
		var condition = new FolderSearchCondition { FolderPath = "/test/dir" };
		condition.IsMatchForSuggest(searchWord).ShouldBe(expected);
	}
}