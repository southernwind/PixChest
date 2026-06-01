using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Stores.Config.Model.Objects;
using MediaDeck.Core.Models.FolderManager;
using MediaDeck.Core.Stores.Config;
using Moq;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.FolderManager;

public class FolderManagerModelTests {
	private static (FolderManagerModel model, Mock<IConfigStore> configStoreMock, FolderManagerConfigModel config) CreateSut() {
		var config = new FolderManagerConfigModel();
		var configStoreMock = new Mock<IConfigStore>();
		// FileRegistrarは具象クラスで複雑な初期化をするため、FolderManagerModelの
		// AddFolder/RemoveFolder/Foldersプロパティの動作テストに絞り、
		// ScanはFileRegistrar統合テストとして別途行う
		// コンストラクタに渡すためnull!を使用（Scan系テストでは呼ばない）
		var model = new FolderManagerModel(null!, config, configStoreMock.Object);
		return (model, configStoreMock, config);
	}

	/// <summary>
	///     AddFolderでフォルダが追加され、Saveが呼ばれることを確認します。
	/// </summary>
	[Fact]
	public void AddFolder_AddsFolderAndSaves() {
		var (model, configStoreMock, config) = CreateSut();

		model.AddFolder("/test/path");

		config.Folders.Count.ShouldBe(1);
		config.Folders[0].FolderPath.ShouldBe("/test/path");
		configStoreMock.Verify(x => x.Save(), Times.Once);
	}

	/// <summary>
	///     RemoveFolderでフォルダが削除され、Saveが呼ばれることを確認します。
	/// </summary>
	[Fact]
	public void RemoveFolder_RemovesFolderAndSaves() {
		var (model, configStoreMock, config) = CreateSut();
		var folder = new FolderModel { FolderPath = "/test/path" };
		config.Folders.Add(folder);

		model.RemoveFolder(folder);

		config.Folders.Count.ShouldBe(0);
		configStoreMock.Verify(x => x.Save(), Times.Once);
	}

	/// <summary>
	///     Foldersプロパティがconfigのフォルダリストを参照していることを確認します。
	/// </summary>
	[Fact]
	public void Folders_ReferencesSameListAsConfig() {
		var (model, _, config) = CreateSut();
		model.Folders.ShouldBeSameAs(config.Folders);
	}
}