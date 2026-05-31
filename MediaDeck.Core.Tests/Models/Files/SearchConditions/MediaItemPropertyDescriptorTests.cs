using System.Linq.Expressions;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

/// <summary>
///     <see cref="MediaItemPropertyDescriptor"/> のテストクラスです。
/// </summary>
public class MediaItemPropertyDescriptorTests {
	[Fact]
	public void Constructor_SetsAllProperties() {
		// Arrange
		var operators = new[] { SearchTypeComparison.Equal, SearchTypeComparison.GreaterThan };
		Expression<Func<MediaItem, bool>>? buildFunc(SearchTypeComparison op, string val) => null;

		// Act
		var descriptor = new MediaItemPropertyDescriptor("TestProp", typeof(int), operators, buildFunc);

		// Assert
		descriptor.Name.ShouldBe("TestProp");
		descriptor.ValueType.ShouldBe(typeof(int));
		descriptor.SupportedOperators.ShouldBe(operators);
	}

	[Fact]
	public void Build_DelegatesToBuildFunction() {
		// Arrange
		Expression<Func<MediaItem, bool>> expected = mi => mi.Rate > 3;
		var descriptor = new MediaItemPropertyDescriptor(
			"Rate",
			typeof(int),
			[SearchTypeComparison.GreaterThan],
			(op, val) => expected);

		// Act
		var result = descriptor.Build(SearchTypeComparison.GreaterThan, "3");

		// Assert
		result.ShouldBe(expected);
	}

	[Fact]
	public void Build_ReturnsNull_WhenBuildFunctionReturnsNull() {
		// Arrange
		var descriptor = new MediaItemPropertyDescriptor(
			"Rate",
			typeof(int),
			[SearchTypeComparison.Equal],
			(op, val) => null);

		// Act
		var result = descriptor.Build(SearchTypeComparison.Equal, "invalid");

		// Assert
		result.ShouldBeNull();
	}
}