using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Files.Filter;
using MediaDeck.Core.Models.Files.Filter.FilterItemObjects;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.Filter;

/// <summary>
/// <see cref="FilterItemFactory"/>のユニットテスト。
/// フィルタオブジェクトを<see cref="FilterItem"/>に変換し、
/// 生成されたExpressionが期待通りに機能することを検証する。
/// </summary>
public class FilterItemFactoryTests {
	private static MediaItem CreateDummyMediaItem() {
		// 必須プロパティを最低限設定したダミーMediaItem
		return new MediaItem {
			MediaType = MediaType.Image, // 適切なMediaTypeを設定
			DirectoryPath = @"C:\\dummy\\path",
			FilePath = @"C:\\dummy\\path\\file.jpg",
			Description = "ダミー",
			IsUnderFolderGroup = false,
			Width = 0,
			Height = 0,
			Rate = 0,
			UsageCount = 0,
			FileSize = 0,
		};
	}

	[Fact]
	public void Create_WithExistsFilterItemObject_ReturnsCorrectExpression() {
		// Arrange
		var existsObject = new ExistsFilterItemObject { Exists = true };

		// Act
		var filterItem = FilterItemFactory.Create(existsObject);

		// Assert
		filterItem.ShouldNotBeNull();
		var func = filterItem.Condition.Compile();
		var mediaItem = CreateDummyMediaItem();
		mediaItem.IsExists = true;
		func(mediaItem).ShouldBeTrue();

		mediaItem.IsExists = false;
		func(mediaItem).ShouldBeFalse();
	}

	[Fact]
	public void Create_WithFilePathFilterItemObject_IncludesText_ReturnsContainsExpression() {
		// Arrange
		var filePathObject = new FilePathFilterItemObject {
			Text = "sample",
			SearchType = SearchTypeInclude.Include
		};

		// Act
		var filterItem = FilterItemFactory.Create(filePathObject);

		// Assert
		filterItem.ShouldNotBeNull();
		var func = filterItem.Condition.Compile();

		var match = CreateDummyMediaItem();
		match.FilePath = @"C:\\path\\to\\sample\\file.txt";
		func(match).ShouldBeTrue();

		var miss = CreateDummyMediaItem();
		miss.FilePath = @"C:\\other\\file.txt";
		func(miss).ShouldBeFalse();
	}

	[Fact]
	public void Create_WithTagFilterItemObject_ReturnsTagContainsExpression() {
		// Arrange
		var tagObject = new TagFilterItemObject {
			TagName = "Nature",
			SearchType = SearchTypeInclude.Include
		};

		// Act
		var filterItem = FilterItemFactory.Create(tagObject);

		// Assert
		filterItem.ShouldNotBeNull();
		var func = filterItem.Condition.Compile();

		var tag = new Tag { TagName = "Nature", Detail = "テスト" };
		var mediaItem = CreateDummyMediaItem();
		mediaItem.MediaItemTags = new[]
		{
			new MediaItemTag { Tag = tag }
		};
		func(mediaItem).ShouldBeTrue();

		var otherTag = new Tag { TagName = "Urban", Detail = "テスト" };
		var otherItem = CreateDummyMediaItem();
		otherItem.MediaItemTags = new[]
		{
			new MediaItemTag { Tag = otherTag }
		};
		func(otherItem).ShouldBeFalse();
	}
}