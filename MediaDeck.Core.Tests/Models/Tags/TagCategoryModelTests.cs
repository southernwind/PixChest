using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Tags;
using Moq;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Tags;

public class TagCategoryModelTests {
	[Fact]
	public void Initialize_WithNull_SetsDefaultCategoryName() {
		var model = new TagCategoryModel();
		var factoryMock = new Mock<ITagModelFactory>();

		model.Initialize(null, factoryMock.Object);

		model.TagCategoryId.ShouldBeNull();
		model.TagCategoryName.ShouldBe("未設定");
		model.Detail.ShouldBe("カテゴリーが設定されていないタグ");
		model.Tags.Count.ShouldBe(0);
		model.IsDirty.ShouldBeFalse();
	}

	[Fact]
	public void Initialize_WithTagCategory_SetsProperties() {
		var model = new TagCategoryModel();
		var factoryMock = new Mock<ITagModelFactory>();
		var tagCategory = new TagCategory {
			TagCategoryId = 1,
			TagCategoryName = "Cat1",
			Detail = "Detail1",
			Tags = []
		};

		model.Initialize(tagCategory, factoryMock.Object);

		model.TagCategoryId.ShouldBe(1);
		model.TagCategoryName.ShouldBe("Cat1");
		model.Detail.ShouldBe("Detail1");
		model.IsDirty.ShouldBeFalse();
	}

	[Fact]
	public void AddTag_AddsToCollection() {
		var model = new TagCategoryModel();
		model.Initialize(null, new Mock<ITagModelFactory>().Object);

		var tagMock = new Mock<ITagModel>();
		model.AddTag(tagMock.Object);

		model.Tags.Count.ShouldBe(1);
	}

	[Fact]
	public void RemoveTag_RemovesFromCollection() {
		var model = new TagCategoryModel();
		model.Initialize(null, new Mock<ITagModelFactory>().Object);

		var tagMock = new Mock<ITagModel>();
		model.AddTag(tagMock.Object);
		model.RemoveTag(tagMock.Object);

		model.Tags.Count.ShouldBe(0);
	}

	[Fact]
	public void ClearTags_ClearsCollection() {
		var model = new TagCategoryModel();
		model.Initialize(null, new Mock<ITagModelFactory>().Object);

		model.AddTag(new Mock<ITagModel>().Object);
		model.AddTag(new Mock<ITagModel>().Object);
		model.ClearTags();

		model.Tags.Count.ShouldBe(0);
	}

	[Fact]
	public void Properties_BeforeInitialize_ThrowInvalidOperationException() {
		var model = new TagCategoryModel();
		Should.Throw<InvalidOperationException>(() => _ = model.TagCategoryId);
		Should.Throw<InvalidOperationException>(() => _ = model.TagCategoryName);
		Should.Throw<InvalidOperationException>(() => _ = model.Detail);
	}

	[Fact]
	public void SetTagCategoryName_SetsIsDirty() {
		var model = new TagCategoryModel();
		model.Initialize(null, new Mock<ITagModelFactory>().Object);

		model.TagCategoryName = "NewName";
		model.IsDirty.ShouldBeTrue();
	}
}