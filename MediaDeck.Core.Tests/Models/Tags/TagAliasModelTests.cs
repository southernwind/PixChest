using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Tags;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Tags;

public class TagAliasModelTests {
	[Fact]
	public void Initialize_SetsAllProperties() {
		var model = new TagAliasModel();
		var alias = new TagAlias { TagAliasId = 5, TagId = 3, Alias = "テスト", Ruby = "てすと" };

		model.Initialize(alias);

		model.TagAliasId.ShouldBe(5);
		model.TagId.ShouldBe(3);
		model.Alias.ShouldBe("テスト");
		model.Ruby.ShouldBe("てすと");
		model.Romaji.ShouldNotBeNullOrEmpty();
	}

	[Fact]
	public void Properties_BeforeInitialize_ThrowInvalidOperationException() {
		var model = new TagAliasModel();
		Should.Throw<InvalidOperationException>(() => _ = model.TagAliasId);
		Should.Throw<InvalidOperationException>(() => _ = model.TagId);
		Should.Throw<InvalidOperationException>(() => _ = model.Alias);
		Should.Throw<InvalidOperationException>(() => _ = model.Romaji);
	}
}