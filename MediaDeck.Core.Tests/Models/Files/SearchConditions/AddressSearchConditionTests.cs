using MediaDeck.Core.Models.Files.SearchConditions;
using MediaDeck.Core.Models.Maps;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

public class AddressSearchConditionTests {
	[Fact]
	public void Address_NotInitialized_ThrowsInvalidOperationException() {
		var condition = new AddressSearchCondition();
		Should.Throw<InvalidOperationException>(() => _ = condition.Address);
	}

	[Fact]
	public void DisplayText_ReturnsAddressName() {
		var condition = new AddressSearchCondition {
			Address = new Address([]) { Name = "Tokyo" }
		};
		condition.DisplayText.ShouldBe("Address=Tokyo");
	}

	[Fact]
	public void WherePredicate_IsNotNull() {
		var condition = new AddressSearchCondition {
			Address = new Address([]) { Name = "Tokyo", IsYet = true }
		};
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Fact]
	public void IsMatchForSuggest_MatchesName() {
		var condition = new AddressSearchCondition {
			Address = new Address([]) { Name = "Tokyo" }
		};
		condition.IsMatchForSuggest("Tok").ShouldBeTrue();
	}

	[Fact]
	public void IsMatchForSuggest_NoMatch_ReturnsFalse() {
		var condition = new AddressSearchCondition {
			Address = new Address([]) { Name = "Tokyo" }
		};
		condition.IsMatchForSuggest("Osaka").ShouldBeFalse();
	}
}