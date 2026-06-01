using MediaDeck.Core.Services.FileChangeMonitor;
using Shouldly;

namespace MediaDeck.Core.Tests.Services.FileChangeMonitor;

/// <summary>
///     <see cref="FileChangeType"/> のテストクラスです。
/// </summary>
public class FileChangeTypeTests {
	[Fact]
	public void FileChangeType_HasExpectedValues() {
		Enum.GetValues<FileChangeType>().Length.ShouldBe(4);
		((int)FileChangeType.Deleted).ShouldBe(0);
		((int)FileChangeType.Renamed).ShouldBe(1);
		((int)FileChangeType.Moved).ShouldBe(2);
		((int)FileChangeType.Added).ShouldBe(3);
	}
}