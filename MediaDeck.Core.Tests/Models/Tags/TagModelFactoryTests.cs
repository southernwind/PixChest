using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Composition.Tables;
using MediaDeck.Core.Models.Tags;
using Moq;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Tags;

public class TagModelFactoryTests {
	private static (TagModelFactory factory, Mock<IServiceProvider> spMock) CreateSut() {
		var spMock = new Mock<IServiceProvider>();
		var factory = new TagModelFactory(spMock.Object);
		return (factory, spMock);
	}

	/// <summary>
	///     Createがサービスプロバイダーからモデルを取得し、Initializeを呼ぶことを確認します。
	/// </summary>
	[Fact]
	public void Create_ResolvesTagModelAndInitializes() {
		var (factory, spMock) = CreateSut();
		var tagModelMock = new Mock<ITagModel>();
		spMock.Setup(x => x.GetService(typeof(ITagModel))).Returns(tagModelMock.Object);

		var categoryMock = new Mock<ITagCategoryModel>();
		var tag = new Tag { TagId = 1, TagName = "Test", TagCategoryId = 1, TagCategory = null!, MediaItemTags = [], TagAliases = [], Detail = "" };

		var result = factory.Create(tag, categoryMock.Object);

		result.ShouldBe(tagModelMock.Object);
		tagModelMock.Verify(x => x.Initialize(tag, categoryMock.Object, factory), Times.Once);
	}

	/// <summary>
	///     CreateCategoryがサービスプロバイダーからモデルを取得し、Initializeを呼ぶことを確認します。
	/// </summary>
	[Fact]
	public void CreateCategory_WithTagCategory_ResolvesAndInitializes() {
		var (factory, spMock) = CreateSut();
		var categoryModelMock = new Mock<ITagCategoryModel>();
		spMock.Setup(x => x.GetService(typeof(ITagCategoryModel))).Returns(categoryModelMock.Object);

		var tagCategory = new TagCategory { TagCategoryId = 1, TagCategoryName = "Cat", Detail = "", Tags = [] };

		var result = factory.CreateCategory(tagCategory);

		result.ShouldBe(categoryModelMock.Object);
		categoryModelMock.Verify(x => x.Initialize(tagCategory, factory), Times.Once);
	}

	/// <summary>
	///     CreateCategory(null)を呼んだ場合もInitialize(null, factory)が呼ばれることを確認します。
	/// </summary>
	[Fact]
	public void CreateCategory_WithNull_InitializesWithNull() {
		var (factory, spMock) = CreateSut();
		var categoryModelMock = new Mock<ITagCategoryModel>();
		spMock.Setup(x => x.GetService(typeof(ITagCategoryModel))).Returns(categoryModelMock.Object);

		var result = factory.CreateCategory();

		result.ShouldBe(categoryModelMock.Object);
		categoryModelMock.Verify(x => x.Initialize(null, factory), Times.Once);
	}

	/// <summary>
	///     CreateAliasがTagAliasでInitializeされることを確認します。
	/// </summary>
	[Fact]
	public void CreateAlias_WithTagAlias_ResolvesAndInitializes() {
		var (factory, spMock) = CreateSut();
		var aliasModelMock = new Mock<ITagAliasModel>();
		spMock.Setup(x => x.GetService(typeof(ITagAliasModel))).Returns(aliasModelMock.Object);

		var alias = new TagAlias { TagAliasId = 1, TagId = 1, Alias = "A1", Ruby = "R1" };

		var result = factory.CreateAlias(alias);

		result.ShouldBe(aliasModelMock.Object);
		aliasModelMock.Verify(x => x.Initialize(alias), Times.Once);
	}

	/// <summary>
	///     CreateAlias()（引数なし）が空のTagAliasでInitializeされることを確認します。
	/// </summary>
	[Fact]
	public void CreateAlias_NoArgs_InitializesWithEmptyAlias() {
		var (factory, spMock) = CreateSut();
		var aliasModelMock = new Mock<ITagAliasModel>();
		spMock.Setup(x => x.GetService(typeof(ITagAliasModel))).Returns(aliasModelMock.Object);

		var result = factory.CreateAlias();

		result.ShouldBe(aliasModelMock.Object);
		aliasModelMock.Verify(x => x.Initialize(It.Is<TagAlias>(a => a.Alias == string.Empty && a.Ruby == string.Empty)), Times.Once);
	}
}