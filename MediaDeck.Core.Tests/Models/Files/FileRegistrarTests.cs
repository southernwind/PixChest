using MediaDeck.Composition.Interfaces.MediaItemTypes;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Stores.Config.Model.Objects;
using MediaDeck.Composition.Stores.State.Model.Objects;
using MediaDeck.Core.Models.Files;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files;

public class FileRegistrarTests {
	private readonly FileRegistrar _sut;
	private readonly ConfigModel _config;

	public FileRegistrarTests() {
		var services = new ServiceCollection();
		services.AddSingleton<PathConfigModel>();
		services.AddSingleton<ExecutionConfigModel>();
		services.AddTransient<ExtensionObjectModel>();
		services.AddSingleton<ScanConfigModel>();
		services.AddSingleton<ThumbnailConfigModel>();
		services.AddSingleton<SearchConfigModel>();
		services.AddSingleton<EnvironmentConfigModel>();
		services.AddSingleton<FolderManagerConfigModel>();
		services.AddSingleton<SearchDefinitionsConfigModel>();
		services.AddSingleton<LanguageConfigModel>();
		services.AddSingleton<MediaDeck.Composition.Interfaces.IStringProvider, StubStringProvider>();
		services.AddSingleton<ConfigModel>();
		services.AddTransient<SortObject>(sp => new SortObject(sp));
		services.AddTransient<SortItemObject>();
		var provider = services.BuildServiceProvider();

		this._config = provider.GetRequiredService<ConfigModel>();
		var logger = new Mock<Microsoft.Extensions.Logging.ILogger<FileRegistrar>>();
		var mediaItemTypeService = new Mock<IMediaItemTypeService>();
		mediaItemTypeService.Setup(x => x.CreateMediaItemOperators()).Returns([]);

		this._sut = new FileRegistrar(this._config, logger.Object, mediaItemTypeService.Object);
	}

	[Fact]
	public void Config_ReturnsSameInstance() {
		this._sut.Config.ShouldBeSameAs(this._config);
	}

	[Fact]
	public void RegistrationQueue_InitiallyEmpty() {
		this._sut.RegistrationQueue.Count.ShouldBe(0);
	}

	[Fact]
	public void Dispose_DoesNotThrow() {
		Should.NotThrow(() => this._sut.Dispose());
	}
}