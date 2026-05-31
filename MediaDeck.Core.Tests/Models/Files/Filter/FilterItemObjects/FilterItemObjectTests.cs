using MediaDeck.Composition.Enum;
using MediaDeck.Core.Models.Files.Filter.FilterItemObjects;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.Filter.FilterItemObjects;

/// <summary>
///     全FilterItemObjectのDisplayNameおよびプロパティのテストクラスです。
/// </summary>
public class FilterItemObjectTests {
	// ─── ExistsFilterItemObject ───

	[Theory]
	[InlineData(true, "File exists")]
	[InlineData(false, "File does not exist")]
	public void ExistsFilterItemObject_DisplayName_ReflectsExists(bool exists, string expected) {
		var obj = new ExistsFilterItemObject { Exists = exists };
		obj.DisplayName.ShouldBe(expected);
	}

	// ─── MediaTypeFilterItemObject ───

	[Theory]
	[InlineData(true, "Video file")]
	[InlineData(false, "Image file")]
	public void MediaTypeFilterItemObject_DisplayName_ReflectsIsVideo(bool isVideo, string expected) {
		var obj = new MediaTypeFilterItemObject { IsVideo = isVideo };
		obj.DisplayName.ShouldBe(expected);
	}

	// ─── FolderGroupFilterItemObject ───

	[Theory]
	[InlineData(SearchTypeInclude.Include, "Include files in folder group")]
	[InlineData(SearchTypeInclude.Exclude, "Exclude files in folder group")]
	public void FolderGroupFilterItemObject_DisplayName_ReflectsSearchType(SearchTypeInclude searchType, string expected) {
		var obj = new FolderGroupFilterItemObject { SearchType = searchType };
		obj.DisplayName.ShouldBe(expected);
	}

	// ─── RateFilterItemObject ───

	[Fact]
	public void RateFilterItemObject_DisplayName_ContainsRateAndComparison() {
		var obj = new RateFilterItemObject { Rate = 3, SearchType = SearchTypeComparison.GreaterThanOrEqual };
		obj.DisplayName.ShouldBe("Rating is 3 greater than or equal to");
	}

	[Fact]
	public void RateFilterItemObject_DisplayName_Equal() {
		var obj = new RateFilterItemObject { Rate = 5, SearchType = SearchTypeComparison.Equal };
		obj.DisplayName.ShouldBe("Rating is 5 equal to");
	}

	// ─── FilePathFilterItemObject ───

	[Fact]
	public void FilePathFilterItemObject_DisplayName_Include() {
		var obj = new FilePathFilterItemObject { Text = "photo", SearchType = SearchTypeInclude.Include };
		obj.DisplayName.ShouldContain("photo");
		obj.DisplayName.ShouldContain("includes");
	}

	[Fact]
	public void FilePathFilterItemObject_DisplayName_Exclude() {
		var obj = new FilePathFilterItemObject { Text = "tmp", SearchType = SearchTypeInclude.Exclude };
		obj.DisplayName.ShouldContain("does not include");
	}

	[Fact]
	public void FilePathFilterItemObject_Text_ThrowsWhenNotInitialized() {
		var obj = new FilePathFilterItemObject();
		Should.Throw<InvalidOperationException>(() => _ = obj.Text);
	}

	// ─── TagFilterItemObject ───

	[Fact]
	public void TagFilterItemObject_DisplayName_Include() {
		var obj = new TagFilterItemObject { TagName = "風景", SearchType = SearchTypeInclude.Include };
		obj.DisplayName.ShouldBe("風景 tag included");
	}

	[Fact]
	public void TagFilterItemObject_DisplayName_Exclude() {
		var obj = new TagFilterItemObject { TagName = "風景", SearchType = SearchTypeInclude.Exclude };
		obj.DisplayName.ShouldBe("風景 tag not included");
	}

	[Fact]
	public void TagFilterItemObject_TagName_ThrowsWhenNotInitialized() {
		var obj = new TagFilterItemObject();
		Should.Throw<InvalidOperationException>(() => _ = obj.TagName);
	}

	// ─── LocationFilterItemObject ───

	[Fact]
	public void LocationFilterItemObject_DisplayName_WithText() {
		var obj = new LocationFilterItemObject { Text = "Tokyo" };
		obj.DisplayName.ShouldBe("Includes Tokyo in place name");
	}

	[Fact]
	public void LocationFilterItemObject_DisplayName_ContainsTrue() {
		var obj = new LocationFilterItemObject { Contains = true };
		obj.DisplayName.ShouldBe("Contains coordinate information");
	}

	[Fact]
	public void LocationFilterItemObject_DisplayName_ContainsFalse() {
		var obj = new LocationFilterItemObject { Contains = false };
		obj.DisplayName.ShouldBe("Does not contain coordinate information");
	}

	// ─── ResolutionFilterItemObject ───

	[Fact]
	public void ResolutionFilterItemObject_DisplayName_Width() {
		var obj = new ResolutionFilterItemObject { Width = 1920, SearchType = SearchTypeComparison.GreaterThanOrEqual };
		obj.DisplayName.ShouldBe("Width is 1920 greater than or equal to");
	}

	[Fact]
	public void ResolutionFilterItemObject_DisplayName_Height() {
		var obj = new ResolutionFilterItemObject { Height = 1080, SearchType = SearchTypeComparison.LessThan };
		obj.DisplayName.ShouldBe("Height is 1080 less than");
	}

	[Fact]
	public void ResolutionFilterItemObject_DisplayName_NoValue_Throws() {
		var obj = new ResolutionFilterItemObject { SearchType = SearchTypeComparison.Equal };
		Should.Throw<InvalidOperationException>(() => _ = obj.DisplayName);
	}
}