using MediaDeck.Core.Models.Files.SearchConditions;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

/// <summary>
///     <see cref="WordSearchCondition"/> のテストクラスです。
/// </summary>
public class WordSearchConditionTests {
	[Fact]
	public void Word_ThrowsWhenNotInitialized() {
		var condition = new WordSearchCondition();
		Should.Throw<InvalidOperationException>(() => _ = condition.Word);
	}

	[Fact]
	public void DisplayText_ShowsWord() {
		var condition = new WordSearchCondition { Word = "sunset" };
		condition.DisplayText.ShouldBe("Word=sunset");
	}

	[Fact]
	public void WherePredicate_IsNotNull() {
		var condition = new WordSearchCondition { Word = "test" };
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Theory]
	[InlineData("sun", true)]
	[InlineData("xyz", false)]
	public void IsMatchForSuggest_MatchesCorrectly(string searchWord, bool expected) {
		var condition = new WordSearchCondition { Word = "sunset" };
		condition.IsMatchForSuggest(searchWord).ShouldBe(expected);
	}
}