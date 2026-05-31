using MediaDeck.Composition.Constants;
using MediaDeck.Core.Services;
using Shouldly;

namespace MediaDeck.Core.Tests.Services;

/// <summary>
///     <see cref="AppPathProvider"/> のテストクラスです。
/// </summary>
public class AppPathProviderTests {
	[Fact]
	public void BaseDirectory_ReturnsFilePathConstant() {
		var provider = new AppPathProvider();
		provider.BaseDirectory.ShouldBe(FilePathConstants.BaseDirectory);
	}

	[Fact]
	public void StateFilePath_ReturnsFilePathConstant() {
		var provider = new AppPathProvider();
		provider.StateFilePath.ShouldBe(FilePathConstants.StateFilePath);
	}

	[Fact]
	public void ConfigFilePath_ReturnsFilePathConstant() {
		var provider = new AppPathProvider();
		provider.ConfigFilePath.ShouldBe(FilePathConstants.ConfigFilePath);
	}

	[Fact]
	public void NoThumbnailFilePath_ReturnsFilePathConstant() {
		var provider = new AppPathProvider();
		provider.NoThumbnailFilePath.ShouldBe(FilePathConstants.NoThumbnailFilePath);
	}
}