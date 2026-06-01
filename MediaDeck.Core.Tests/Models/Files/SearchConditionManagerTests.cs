using MediaDeck.Composition.Database;
using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Interfaces.Notifications;
using MediaDeck.Composition.Interfaces.Tags;
using MediaDeck.Composition.Stores.State.Model;
using MediaDeck.Core.Models.Files;
using MediaDeck.Core.Models.Files.SearchConditions;
using MediaDeck.Core.Models.Repositories;
using MediaDeck.Core.Models.Repositories.Objects;
using Microsoft.EntityFrameworkCore;
using Moq;
using ObservableCollections;
using R3;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files;

public class SearchConditionManagerTests {
	private static IDbContextFactory<MediaDeckDbContext> CreateInMemoryDbFactory(string dbName) {
		var options = new DbContextOptionsBuilder<MediaDeckDbContext>()
			.UseInMemoryDatabase(databaseName: dbName)
			.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
			.Options;

		var factoryMock = new Mock<IDbContextFactory<MediaDeckDbContext>>();
		factoryMock.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(() => new MediaDeckDbContext(options));

		return factoryMock.Object;
	}

	private static (SearchConditionManager manager, Subject<ISearchCondition> addSubject, Subject<ISearchCondition> removeSubject, Subject<Action<ObservableList<ISearchCondition>>> updateSubject) CreateSut(string testName) {
		var addSubject = new Subject<ISearchCondition>();
		var removeSubject = new Subject<ISearchCondition>();
		var updateSubject = new Subject<Action<ObservableList<ISearchCondition>>>();

		var dispatcherMock = new Mock<ISearchConditionNotificationDispatcher>();
		dispatcherMock.Setup(x => x.AddRequest).Returns(addSubject);
		dispatcherMock.Setup(x => x.RemoveRequest).Returns(removeSubject);
		dispatcherMock.Setup(x => x.UpdateRequest).Returns(updateSubject);

		var tagsManagerMock = new Mock<ITagsManager>();
		tagsManagerMock.Setup(x => x.Tags).Returns(new ObservableList<ITagModel>());

		// FolderRepositoryにInMemory DBを渡して空のルートフォルダを返す状態にする
		var dbFactory = CreateInMemoryDbFactory(testName);
		var folderDispatcherMock = new Mock<ISearchConditionNotificationDispatcher>();
		folderDispatcherMock.Setup(x => x.UpdateRequest).Returns(new Subject<Action<ObservableList<ISearchCondition>>>());
		var tabState = new TabStateModel(null!, new SearchStateModel(), new ViewerStateModel());
		var folderRepo = new FolderRepository(dbFactory, folderDispatcherMock.Object, tabState);
		folderRepo.RootFolder.Value = new FolderObject(null, "", []);

		var manager = new SearchConditionManager(dispatcherMock.Object, tagsManagerMock.Object, folderRepo, tabState);
		return (manager, addSubject, removeSubject, updateSubject);
	}

	/// <summary>
	///     AddRequestで検索条件が追加されることを確認します。
	/// </summary>
	[Fact]
	public void AddRequest_AddsSearchCondition() {
		var (manager, addSubject, _, _) = CreateSut(nameof(AddRequest_AddsSearchCondition));

		var condition = new WordSearchCondition { Word = "test" };
		addSubject.OnNext(condition);

		manager.SearchConditions.ShouldContain(condition);
	}

	/// <summary>
	///     RemoveRequestで検索条件が削除されることを確認します。
	/// </summary>
	[Fact]
	public void RemoveRequest_RemovesSearchCondition() {
		var (manager, addSubject, removeSubject, _) = CreateSut(nameof(RemoveRequest_RemovesSearchCondition));

		var condition = new WordSearchCondition { Word = "test" };
		addSubject.OnNext(condition);
		removeSubject.OnNext(condition);

		manager.SearchConditions.ShouldNotContain(condition);
	}

	/// <summary>
	///     UpdateRequestでリストを操作できることを確認します。
	/// </summary>
	[Fact]
	public void UpdateRequest_ModifiesSearchConditions() {
		var (manager, _, _, updateSubject) = CreateSut(nameof(UpdateRequest_ModifiesSearchConditions));

		var condition = new WordSearchCondition { Word = "injected" };
		updateSubject.OnNext(list => list.Add(condition));

		manager.SearchConditions.ShouldContain(condition);
	}

	/// <summary>
	///     SearchConditionCandidatesにPropertySearchConditionのサジェストが含まれることを確認します。
	/// </summary>
	[Fact]
	public void Candidates_ContainPropertySearchConditions() {
		var (manager, _, _, _) = CreateSut(nameof(Candidates_ContainPropertySearchConditions));

		manager.SearchConditionCandidates.OfType<PropertySearchCondition>().ShouldNotBeEmpty();
	}
}