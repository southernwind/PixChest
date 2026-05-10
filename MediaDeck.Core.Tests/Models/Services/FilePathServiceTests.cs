using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Stores.Config.Model.Objects;
using MediaDeck.Composition.Stores.State.Model.Objects;
using MediaDeck.Core.Services;

using Microsoft.Extensions.DependencyInjection;

using Shouldly;

namespace MediaDeck.Core.Tests.Models.Services;

public class FilePathServiceTests {
	private readonly FilePathService _sut;
	private readonly ConfigModel _config;

	public FilePathServiceTests() {
		var services = new ServiceCollection();

		services.AddSingleton<PathConfigModel>();
		services.AddSingleton<ExecutionConfigModel>();
		services.AddTransient<ExtensionObjectModel>();
		services.AddSingleton<ScanConfigModel>();
		services.AddSingleton<ThumbnailConfigModel>();
		services.AddSingleton<SearchConfigModel>();
		services.AddSingleton<FolderManagerConfigModel>();
		services.AddSingleton<SearchDefinitionsConfigModel>();
		services.AddSingleton<LanguageConfigModel>();
		services.AddSingleton<MediaDeck.Composition.Interfaces.IStringProvider, StubStringProvider>();
		services.AddSingleton<ConfigModel>();
		services.AddTransient<SortObject>(sp => new SortObject(sp));
		services.AddTransient<SortItemObject>();

		var provider = services.BuildServiceProvider();

		this._config = provider.GetRequiredService<ConfigModel>();
		this._sut = new FilePathService(this._config);
	}

	[Fact]
	public void GetThumbnailRelativeFilePath_ShouldReturnGuidBasedPath() {
		// Act
		var result = this._sut.GetThumbnailRelativeFilePath();

		// Assert
		result.ShouldNotBeNullOrWhiteSpace();
		result.ShouldEndWith(".jpg");

		// Format should be: [2 chars]\[30 chars].jpg
		// Guid without hyphens ("N") is 32 chars. Total length = 2 + 1 + 30 + 4 = 37
		result.Length.ShouldBe(37);
		result.Substring(2, 1).ShouldBe(@"\");
	}

	[Fact]
	public void GetThumbnailAbsoluteFilePath_ShouldReturnCombinedPath() {
		// Arrange
		var relativePath = @"1a\bcdef0123456789abcdef0123456789.jpg";
		var size = this._config.ThumbnailConfig.ThumbnailSize.Value;
		var expectedBasePath = this._config.PathConfig.ThumbnailFolderPath.Value;
		var expectedAbsolutePath = Path.Combine(expectedBasePath, size.ToString(), relativePath);

		// Act
		var result = this._sut.GetThumbnailAbsoluteFilePath(relativePath);

		// Assert
		result.ShouldBe(expectedAbsolutePath);
	}

	[Fact]
	public void GetThumbnailAbsoluteFilePath_WithSpecifiedSize_ShouldReturnCombinedPath() {
		// Arrange
		var relativePath = @"1a\bcdef0123456789abcdef0123456789.jpg";
		var size = 128;
		var expectedBasePath = this._config.PathConfig.ThumbnailFolderPath.Value;
		var expectedAbsolutePath = Path.Combine(expectedBasePath, "128", relativePath);

		// Act
		var result = this._sut.GetThumbnailAbsoluteFilePath(relativePath, size);

		// Assert
		result.ShouldBe(expectedAbsolutePath);
	}

}