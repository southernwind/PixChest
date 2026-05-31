using MediaDeck.Composition.Enum;
using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

/// <summary>
///     <see cref="PropertySearchCondition"/> のテストクラスです。
/// </summary>
public class PropertySearchConditionTests {
	[Fact]
	public void PropertyName_ThrowsWhenNotInitialized() {
		var condition = new PropertySearchCondition();
		Should.Throw<InvalidOperationException>(() => _ = condition.PropertyName);
	}

	[Fact]
	public void DisplayText_WhenNotConfigured_ShowsStub() {
		var condition = new PropertySearchCondition { PropertyName = "Rate", IsConfigured = false };
		condition.DisplayText.ShouldBe("prop.Rate");
	}

	[Fact]
	public void DisplayText_WhenConfigured_ShowsFullExpression() {
		var condition = new PropertySearchCondition {
			PropertyName = "Rate",
			IsConfigured = true,
			Operator = SearchTypeComparison.GreaterThan,
			Value = "3"
		};
		condition.DisplayText.ShouldBe("prop.Rate > 3");
	}

	[Fact]
	public void DisplayText_EqualOperator_ShowsEqualSign() {
		var condition = new PropertySearchCondition {
			PropertyName = "Width",
			IsConfigured = true,
			Operator = SearchTypeComparison.Equal,
			Value = "1920"
		};
		condition.DisplayText.ShouldBe("prop.Width = 1920");
	}

	[Fact]
	public void WherePredicate_WhenNotConfigured_ReturnsNull() {
		var condition = new PropertySearchCondition { PropertyName = "Rate", IsConfigured = false };
		condition.WherePredicate.ShouldBeNull();
	}

	[Fact]
	public void WherePredicate_WhenConfiguredWithValidProperty_ReturnsExpression() {
		var condition = new PropertySearchCondition {
			PropertyName = "Rate",
			IsConfigured = true,
			Operator = SearchTypeComparison.Equal,
			Value = "3"
		};
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Theory]
	[InlineData("prop.Rate", true)]
	[InlineData("Rate", true)]
	[InlineData("xyz", false)]
	public void IsMatchForSuggest_MatchesCorrectly(string searchWord, bool expected) {
		var condition = new PropertySearchCondition { PropertyName = "Rate" };
		condition.IsMatchForSuggest(searchWord).ShouldBe(expected);
	}
}