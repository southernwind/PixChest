using MediaDeck.Composition.Database;
using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Composition.Interfaces.Threading;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Stores.State.Model;
using MediaDeck.Composition.Stores.State.Model.Objects;
using MediaDeck.Core.Models.Files;
using MediaDeck.Core.Models.Files.Loaders;
using MediaDeck.Core.Models.NotificationDispatcher;
using MediaDeck.Core.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files;

public class MediaContentLibraryTests {
	private readonly MediaContentLibrary _sut;

	public MediaContentLibraryTests() {
		var filesLoader = new Mock<FilesLoader>(
			MockBehavior.Loose,
			null!, null!, null!, null!).Object;
		var searchConfig = new SearchConfigModel();

		// Build TabStateModel via DI
		var services = new ServiceCollection();
		services.AddTransient<SortObject>(sp => new SortObject(sp));
		services.AddTransient<SortItemObject>();
		services.AddSingleton<SearchDefinitionsConfigModel>();
		services.AddSingleton<SearchStateModel>();
		services.AddSingleton<ViewerStateModel>();
		services.AddSingleton<TabStateModel>();
		services.AddSingleton<MediaDeck.Composition.Interfaces.IStringProvider, StubStringProvider>();
		var sp = services.BuildServiceProvider();
		var tabState = sp.GetRequiredService<TabStateModel>();

		var dispatcher = new SearchConditionNotificationDispatcher();
		var tagsManager = new Mock<ITagsManager>();
		tagsManager.Setup(x => x.Tags).Returns(new ObservableCollections.ObservableList<MediaDeck.Composition.Interfaces.Tags.ITagModel>());

		// Real FolderRepository with InMemory DB
		var dbOptions = new DbContextOptionsBuilder<MediaDeckDbContext>()
			.UseInMemoryDatabase($"MCL_{Guid.NewGuid()}")
			.Options;
		var dbFactoryMock = new Mock<IDbContextFactory<MediaDeckDbContext>>();
		dbFactoryMock.Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(() => new MediaDeckDbContext(dbOptions));
		var folderRepo = new FolderRepository(dbFactoryMock.Object, dispatcher, tabState);

		var searchConditionManager = new SearchConditionManager(dispatcher, tagsManager.Object, folderRepo, tabState);

		var uiDispatcher = new Mock<IUiDispatcher>();

		this._sut = new MediaContentLibrary(filesLoader, searchConfig, searchConditionManager, dispatcher, uiDispatcher.Object);
	}

	[Fact]
	public void Files_InitiallyEmpty() {
		this._sut.Files.ShouldBeEmpty();
	}

	[Fact]
	public void CanLoadMore_DefaultFalse() {
		this._sut.CanLoadMore.Value.ShouldBeFalse();
	}

	[Fact]
	public void SearchElapsedMilliseconds_DefaultNull() {
		this._sut.SearchElapsedMilliseconds.Value.ShouldBeNull();
	}

	[Fact]
	public void TotalCount_DefaultNull() {
		this._sut.TotalCount.Value.ShouldBeNull();
	}

	[Fact]
	public void Dispose_DoesNotThrow() {
		Should.NotThrow(() => this._sut.Dispose());
	}
}