using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Core.Models.Files.SearchConditions;
using Moq;
using ObservableCollections;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.SearchConditions;

public class TagSearchConditionTests {
	private static (TagSearchCondition condition, Mock<ITagsManager> tagsManagerMock) CreateSut(int tagId = 1) {
		var tagModelMock = new Mock<ITagModel>();
		tagModelMock.Setup(x => x.TagId).Returns(tagId);
		tagModelMock.Setup(x => x.TagName).Returns("TestTag");
		tagModelMock.Setup(x => x.Ruby).Returns("テストタグ");
		tagModelMock.Setup(x => x.Romaji).Returns("tesutotagu");
		tagModelMock.Setup(x => x.TagAliases).Returns(new List<ITagAliasModel>());

		var tagsManagerMock = new Mock<ITagsManager>();
		tagsManagerMock.Setup(x => x.Tags).Returns(new ObservableList<ITagModel>([tagModelMock.Object]));

		var condition = new TagSearchCondition(tagsManagerMock.Object) { TagId = tagId };
		return (condition, tagsManagerMock);
	}

	[Fact]
	public void DisplayText_ReturnsTagName() {
		var (condition, _) = CreateSut();
		condition.DisplayText.ShouldBe("TagName=TestTag");
	}

	[Fact]
	public void DisplayText_WithRepresentativeText_AppendsIt() {
		var (condition, _) = CreateSut();
		condition.RepresentativeText = "Alias1";
		condition.DisplayText.ShouldBe("TagName=TestTag (Alias1)");
	}

	[Fact]
	public void DisplayText_TagNotFound_ReturnsTagId() {
		var tagsManagerMock = new Mock<ITagsManager>();
		tagsManagerMock.Setup(x => x.Tags).Returns(new ObservableList<ITagModel>());
		var condition = new TagSearchCondition(tagsManagerMock.Object) { TagId = 999 };

		condition.DisplayText.ShouldBe("TagId=999");
	}

	[Fact]
	public void WherePredicate_IsNotNull() {
		var (condition, _) = CreateSut();
		condition.WherePredicate.ShouldNotBeNull();
	}

	[Fact]
	public void IsMatchForSuggest_MatchesTagName() {
		var (condition, _) = CreateSut();
		condition.IsMatchForSuggest("Test").ShouldBeTrue();
	}

	[Fact]
	public void IsMatchForSuggest_MatchesRuby() {
		var (condition, _) = CreateSut();
		condition.IsMatchForSuggest("テスト").ShouldBeTrue();
	}

	[Fact]
	public void IsMatchForSuggest_NoMatch_ReturnsFalse() {
		var (condition, _) = CreateSut();
		condition.IsMatchForSuggest("zzz").ShouldBeFalse();
	}
}