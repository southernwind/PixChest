using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Stores.State.Model.Objects;
using MediaDeck.Core.Models.Files.Filter;
using MediaDeck.Core.Models.Files.Filter.FilterItemObjects;
using Moq;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.Filter;

/// <summary>
/// FilteringConditionEditorクラスのテスト
/// 責務:
/// 1. フィルター設定オブジェクト(FilterObject)を保持し、操作のベースとする
/// 2. 各種特定のフィルター条件(Tag, FilePath, Rate, Resolution, MediaType, Location, Existsなど)を容易に追加するメソッドを提供する
/// 3. 追加・削除などの変更が、基となるFilterObjectのFilterItemObjectsコレクションに正しく反映される
/// </summary>
public class FilteringConditionEditorTests {
	/// <summary>
	/// コンストラクタが正しく初期化され、FilterObjectのプロパティが適切にマッピングされることを検証する
	/// </summary>
	[Fact]
	public void Constructor_ShouldInitializePropertiesCorrectly() {
		// Arrange
		var filterObject = new FilterObject();
		filterObject.DisplayName.Value = "Test Display Name";
		var dummyFilter = new Mock<IFilterItemObject>();
		filterObject.FilterItemObjects.Add(dummyFilter.Object);

		// Act
		using var editor = new FilteringConditionEditor(filterObject);

		// Assert
		editor.FilterObject.ShouldBe(filterObject);
		editor.DisplayName.Value.ShouldBe("Test Display Name");
		editor.FilterItemObjects.Count.ShouldBe(1);
		editor.FilterItemObjects[0].ShouldBe(dummyFilter.Object);
	}

	/// <summary>
	/// AddFilterメソッドが任意のIFilterItemObjectを追加できることを検証する
	/// </summary>
	[Fact]
	public void AddFilter_ShouldAddAnyFilterItemObject() {
		// Arrange
		var filterObject = new FilterObject();
		using var editor = new FilteringConditionEditor(filterObject);
		var dummyFilter = new Mock<IFilterItemObject>();

		// Act
		editor.AddFilter(dummyFilter.Object);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		editor.FilterItemObjects[0].ShouldBe(dummyFilter.Object);
	}

	/// <summary>
	/// AddTagFilterメソッドが正しいプロパティを持つTagFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddTagFilter_ShouldAddTagFilterItemObject() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var tagName = "test-tag";
		var searchType = SearchTypeInclude.Include;

		// Act
		editor.AddTagFilter(tagName, searchType);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<TagFilterItemObject>();
		addedFilter.TagName.ShouldBe(tagName);
		addedFilter.SearchType.ShouldBe(searchType);
	}

	/// <summary>
	/// AddFilePathFilterメソッドが正しいプロパティを持つFilePathFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddFilePathFilter_ShouldAddFilePathFilterItemObject() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var text = "folder1\\file";
		var searchType = SearchTypeInclude.Exclude;

		// Act
		editor.AddFilePathFilter(text, searchType);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<FilePathFilterItemObject>();
		addedFilter.Text.ShouldBe(text);
		addedFilter.SearchType.ShouldBe(searchType);
	}

	/// <summary>
	/// AddRateFilterメソッドが正しいプロパティを持つRateFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddRateFilter_ShouldAddRateFilterItemObject() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var rate = 5;
		var searchType = SearchTypeComparison.GreaterThanOrEqual;

		// Act
		editor.AddRateFilter(rate, searchType);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<RateFilterItemObject>();
		addedFilter.Rate.ShouldBe(rate);
		addedFilter.SearchType.ShouldBe(searchType);
	}

	/// <summary>
	/// AddResolutionFilterメソッドが両方のサイズ（幅・高さ）指定時に正しいComparableSizeを持つResolutionFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddResolutionFilter_WithWidthAndHeight_ShouldAddResolutionFilterItemObjectWithComparableSize() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var width = 1920;
		var height = 1080;
		var searchType = SearchTypeComparison.Equal;

		// Act
		editor.AddResolutionFilter(width, height, searchType);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<ResolutionFilterItemObject>();
		addedFilter.SearchType.ShouldBe(searchType);
		addedFilter.Resolution.ShouldNotBeNull();
		addedFilter.Resolution.Value.Width.ShouldBe(width);
		addedFilter.Resolution.Value.Height.ShouldBe(height);
	}

	/// <summary>
	/// AddResolutionFilterメソッドが片方のサイズのみ指定時に正しいプロパティを持つResolutionFilterItemObjectを追加することを検証する
	/// </summary>
	[Theory]
	[InlineData(1920, null)]
	[InlineData(null, 1080)]
	public void AddResolutionFilter_WithPartialSize_ShouldAddResolutionFilterItemObjectWithWidthOrHeight(int? width, int? height) {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var searchType = SearchTypeComparison.Equal;

		// Act
		editor.AddResolutionFilter(width, height, searchType);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<ResolutionFilterItemObject>();
		addedFilter.SearchType.ShouldBe(searchType);
		addedFilter.Resolution.ShouldBeNull();
		addedFilter.Width.ShouldBe(width);
		addedFilter.Height.ShouldBe(height);
	}

	/// <summary>
	/// AddMediaTypeFilterメソッドが正しいプロパティを持つMediaTypeFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddMediaTypeFilter_ShouldAddMediaTypeFilterItemObject() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var isVideo = true;

		// Act
		editor.AddMediaTypeFilter(isVideo);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<MediaTypeFilterItemObject>();
		addedFilter.IsVideo.ShouldBe(isVideo);
	}

	/// <summary>
	/// AddLocationFilterメソッドが正しいプロパティを持つLocationFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddLocationFilter_ShouldAddLocationFilterItemObject() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var hasLocation = true;

		// Act
		editor.AddLocationFilter(hasLocation);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<LocationFilterItemObject>();
		addedFilter.Contains.ShouldBe(hasLocation);
	}

	/// <summary>
	/// AddExistsFilterメソッドが正しいプロパティを持つExistsFilterItemObjectを追加することを検証する
	/// </summary>
	[Fact]
	public void AddExistsFilter_ShouldAddExistsFilterItemObject() {
		// Arrange
		using var editor = new FilteringConditionEditor(new FilterObject());
		var exists = false;

		// Act
		editor.AddExistsFilter(exists);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		var addedFilter = editor.FilterItemObjects[0].ShouldBeOfType<ExistsFilterItemObject>();
		addedFilter.Exists.ShouldBe(exists);
	}

	/// <summary>
	/// RemoveFilterメソッドが指定されたフィルターをコレクションから削除できることを検証する
	/// </summary>
	[Fact]
	public void RemoveFilter_ShouldRemoveSpecifiedFilterItemObject() {
		// Arrange
		var filterObject = new FilterObject();
		var filter1 = new Mock<IFilterItemObject>().Object;
		var filter2 = new Mock<IFilterItemObject>().Object;
		filterObject.FilterItemObjects.Add(filter1);
		filterObject.FilterItemObjects.Add(filter2);

		using var editor = new FilteringConditionEditor(filterObject);

		// Act
		editor.RemoveFilter(filter1);

		// Assert
		editor.FilterItemObjects.Count.ShouldBe(1);
		editor.FilterItemObjects[0].ShouldBe(filter2);
	}
}