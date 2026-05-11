using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

public class MediaItemPropertyCatalogTests {
	/// <summary>
	/// 大文字小文字を無視してプロパティ記述子を取得できること
	/// </summary>
	[Theory]
	[InlineData("rate")]
	[InlineData("RATE")]
	[InlineData("Rate")]
	public void Find_ReturnsDescriptorIgnoringCase(string propertyName) {
		var descriptor = MediaItemPropertyCatalog.Find(propertyName);

		descriptor.ShouldNotBeNull();
		descriptor.Name.ShouldBe("Rate");
	}

	/// <summary>
	/// 存在しないプロパティ名を指定した場合は null を返すこと
	/// </summary>
	[Fact]
	public void Find_UnknownProperty_ReturnsNull() {
		var descriptor = MediaItemPropertyCatalog.Find("UnknownProperty");

		descriptor.ShouldBeNull();
	}

	/// <summary>
	/// 数値型プロパティに対して正しい比較式が生成されること
	/// </summary>
	[Fact]
	public void Build_ComparableProperty_ValidValue_ReturnsPredicate() {
		var descriptor = MediaItemPropertyCatalog.Find("Rate");
		descriptor.ShouldNotBeNull();

		// Rate >= 3
		var expression = descriptor.Build(SearchTypeComparison.GreaterThanOrEqual, "3");
		expression.ShouldNotBeNull();

		var predicate = expression.Compile();

		var itemMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "", MediaType = MediaType.Image, IsUnderFolderGroup = false, Rate = 4 };
		var itemNotMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "", MediaType = MediaType.Image, IsUnderFolderGroup = false, Rate = 2 };

		predicate(itemMatch).ShouldBeTrue();
		predicate(itemNotMatch).ShouldBeFalse();
	}

	/// <summary>
	/// 数値として不正な文字列の場合は null を返すこと
	/// </summary>
	[Fact]
	public void Build_ComparableProperty_InvalidValue_ReturnsNull() {
		var descriptor = MediaItemPropertyCatalog.Find("Rate");
		descriptor.ShouldNotBeNull();

		var expression = descriptor.Build(SearchTypeComparison.Equal, "InvalidNumber");

		expression.ShouldBeNull();
	}

	/// <summary>
	/// 列挙型プロパティに対して正しい比較式が生成されること
	/// </summary>
	[Fact]
	public void Build_EnumProperty_ValidValue_ReturnsPredicate() {
		var descriptor = MediaItemPropertyCatalog.Find("MediaType");
		descriptor.ShouldNotBeNull();

		// MediaType = Image
		var expression = descriptor.Build(SearchTypeComparison.Equal, "Image");
		expression.ShouldNotBeNull();

		var predicate = expression.Compile();

		var itemMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "", MediaType = MediaType.Image, IsUnderFolderGroup = false };
		var itemNotMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "", MediaType = MediaType.Video, IsUnderFolderGroup = false };

		predicate(itemMatch).ShouldBeTrue();
		predicate(itemNotMatch).ShouldBeFalse();
	}

	/// <summary>
	/// 真偽値プロパティに対して正しい比較式が生成されること
	/// </summary>
	[Fact]
	public void Build_BoolProperty_ValidValue_ReturnsPredicate() {
		var descriptor = MediaItemPropertyCatalog.Find("IsExists");
		descriptor.ShouldNotBeNull();

		// IsExists = true
		var expression = descriptor.Build(SearchTypeComparison.Equal, "true");
		expression.ShouldNotBeNull();

		var predicate = expression.Compile();

		var itemMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "", MediaType = MediaType.Image, IsUnderFolderGroup = false, IsExists = true };
		var itemNotMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "", MediaType = MediaType.Image, IsUnderFolderGroup = false, IsExists = false };

		predicate(itemMatch).ShouldBeTrue();
		predicate(itemNotMatch).ShouldBeFalse();
	}

	/// <summary>
	/// 文字列型プロパティに対してContainsの比較式が生成されること
	/// </summary>
	[Fact]
	public void Build_StringProperty_Contains_ReturnsPredicate() {
		var descriptor = MediaItemPropertyCatalog.Find("Description");
		descriptor.ShouldNotBeNull();

		// Description contains "keyword"
		var expression = descriptor.Build(SearchTypeComparison.Contains, "keyword");
		expression.ShouldNotBeNull();

		var predicate = expression.Compile();

		var itemMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "This is a keyword test.", MediaType = MediaType.Image, IsUnderFolderGroup = false };
		var itemNotMatch = new MediaItem { DirectoryPath = "", FilePath = "", Description = "This is another test.", MediaType = MediaType.Image, IsUnderFolderGroup = false };

		predicate(itemMatch).ShouldBeTrue();
		predicate(itemNotMatch).ShouldBeFalse();
	}
}