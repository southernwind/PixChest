using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;
using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Core.Models.Files;
using Moq;
using ObservableCollections;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files;

public class DetailSelectorModelTests : IDisposable {
	private readonly DetailSelectorModel _sut;

	public DetailSelectorModelTests() {
		var tagsManagerMock = new Mock<ITagsManager>();
		tagsManagerMock.Setup(x => x.Tags).Returns(new ObservableList<ITagModel>());
		tagsManagerMock.Setup(x => x.TagCategories).Returns(new ObservableList<ITagCategoryModel>());
		this._sut = new DetailSelectorModel(tagsManagerMock.Object);
	}

	public void Dispose() {
		this._sut.Dispose();
	}

	[Fact]
	public void Tags_IsInitiallyEmpty() {
		this._sut.Tags.Count.ShouldBe(0);
	}

	[Fact]
	public void Properties_IsInitiallyEmpty() {
		this._sut.Properties.Value.ShouldBeEmpty();
	}

	[Fact]
	public void Rate_DefaultIsZero() {
		this._sut.Rate.Value.ShouldBe(0);
	}

	[Fact]
	public void RepresentativeFilePath_DefaultIsEmpty() {
		this._sut.RepresentativeFilePath.Value.ShouldBeEmpty();
	}

	[Fact]
	public void Refresh_EmptyArray_ClearsProperties() {
		this._sut.Refresh([]);
		this._sut.Properties.Value.ShouldBeEmpty();
		this._sut.Rate.Value.ShouldBe(0);
		this._sut.UsageCount.Value.ShouldBe(0);
	}

	[Fact]
	public void Refresh_WithFile_SetsProperties() {
		var fileMock = new Mock<IMediaItemModel>();
		fileMock.Setup(x => x.Properties).Returns([]);
		fileMock.Setup(x => x.Rate).Returns(4);
		fileMock.Setup(x => x.UsageCount).Returns(10);
		fileMock.Setup(x => x.FilePath).Returns("/test.jpg");
		fileMock.Setup(x => x.Description).Returns("desc");
		fileMock.Setup(x => x.Tags).Returns(new List<ITagModel>());
		fileMock.Setup(x => x.Changed).Returns(R3.Observable.Empty<R3.Unit>());

		this._sut.Refresh([fileMock.Object]);

		this._sut.Rate.Value.ShouldBe(4);
		this._sut.UsageCount.Value.ShouldBe(10);
		this._sut.RepresentativeFilePath.Value.ShouldBe("/test.jpg");
		this._sut.Description.Value.ShouldBe("desc");
	}

	[Fact]
	public void ContentChanged_IsNotNull() {
		this._sut.ContentChanged.ShouldNotBeNull();
	}

	[Fact]
	public void TagsManager_ReturnsMockedInstance() {
		this._sut.TagsManager.ShouldNotBeNull();
	}
}