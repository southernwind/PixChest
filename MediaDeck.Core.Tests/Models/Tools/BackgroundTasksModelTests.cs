using MediaDeck.Composition.Interfaces.Services;
using MediaDeck.Core.Models.Tools;
using MediaDeck.Core.Services.FileStatusUpdator;
using MediaDeck.Core.Services.MediaItemMetadataUpdator;

using Moq;

using Shouldly;

namespace MediaDeck.Core.Tests.Models.Tools;

public class BackgroundTasksModelTests {
	private readonly BackgroundTasksModel _sut;

	public BackgroundTasksModelTests() {
		var dbFactory = new Mock<Microsoft.EntityFrameworkCore.IDbContextFactory<MediaDeck.Composition.Database.MediaDeckDbContext>>();
		var hashService = new Mock<IFileHashUpdatorService>();
		hashService.Setup(x => x.CompletedCount).Returns(new R3.ReactiveProperty<long>());
		hashService.Setup(x => x.TargetCount).Returns(new R3.ReactiveProperty<long>());
		hashService.Setup(x => x.FullHashCompletedCount).Returns(new R3.ReactiveProperty<long>());
		hashService.Setup(x => x.FullHashTargetCount).Returns(new R3.ReactiveProperty<long>());

		var mediaItemTypeService = new Mock<MediaDeck.Composition.Interfaces.MediaItemTypes.IMediaItemTypeService>();
		var dbWriteCoord = new Mock<IDatabaseWriteCoordinator>();

		var fileStatusUpdator = new FileStatusUpdatorService(dbFactory.Object, hashService.Object, mediaItemTypeService.Object, dbWriteCoord.Object);
		var metadataUpdator = new MediaItemMetadataUpdatorService(dbFactory.Object, mediaItemTypeService.Object);

		this._sut = new BackgroundTasksModel(fileStatusUpdator, hashService.Object, metadataUpdator);
	}

	[Fact]
	public void TaskItems_HasFourItems() {
		this._sut.TaskItems.Count.ShouldBe(4);
	}

	[Fact]
	public void TaskItems_FirstIsUpdateFileStatus() {
		this._sut.TaskItems[0].DisplayName.ShouldBe("Update file status");
	}

	[Fact]
	public void TaskItems_SecondIsUpdateFileHash() {
		this._sut.TaskItems[1].DisplayName.ShouldBe("Update file hash");
	}

	[Fact]
	public void TaskItems_ThirdIsUpdateFullHash() {
		this._sut.TaskItems[2].DisplayName.ShouldBe("Update full hash");
	}

	[Fact]
	public void TaskItems_FourthIsUpdateMetadata() {
		this._sut.TaskItems[3].DisplayName.ShouldBe("Update metadata");
	}

	[Fact]
	public void Actions_IsNotNull() {
		this._sut.Actions.ShouldNotBeNull();
	}

	[Fact]
	public void Dispose_DoesNotThrow() {
		Should.NotThrow(() => this._sut.Dispose());
	}
}