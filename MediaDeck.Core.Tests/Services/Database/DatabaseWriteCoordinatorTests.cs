using MediaDeck.Core.Services.Database;
using Shouldly;

namespace MediaDeck.Core.Tests.Services.Database;

/// <summary>
///     <see cref="DatabaseWriteCoordinator"/> のテストクラスです。
/// </summary>
public class DatabaseWriteCoordinatorTests {
	/// <summary>
	///     ExecuteAsyncが操作を正しく実行することを確認します。
	/// </summary>
	[Fact]
	public async Task ExecuteAsync_RunsOperation() {
		// Arrange
		using var coordinator = new DatabaseWriteCoordinator();
		var executed = false;

		// Act
		await coordinator.ExecuteAsync(async ct => {
			await Task.Yield();
			executed = true;
		});

		// Assert
		executed.ShouldBeTrue();
	}

	/// <summary>
	///     ExecuteAsync（戻り値あり）が結果を正しく返すことを確認します。
	/// </summary>
	[Fact]
	public async Task ExecuteAsync_WithResult_ReturnsValue() {
		// Arrange
		using var coordinator = new DatabaseWriteCoordinator();

		// Act
		var result = await coordinator.ExecuteAsync(async ct => {
			await Task.Yield();
			return 42;
		});

		// Assert
		result.ShouldBe(42);
	}

	/// <summary>
	///     複数の操作が直列に実行されることを確認します。
	/// </summary>
	[Fact]
	public async Task ExecuteAsync_SerializesOperations() {
		// Arrange
		using var coordinator = new DatabaseWriteCoordinator();
		var order = new List<int>();

		// Act
		var task1 = coordinator.ExecuteAsync(async ct => {
			await Task.Delay(50, ct);
			order.Add(1);
		});
		var task2 = coordinator.ExecuteAsync(async ct => {
			order.Add(2);
		});

		await Task.WhenAll(task1, task2);

		// Assert
		order.ShouldBe([1, 2]);
	}

	/// <summary>
	///     nullの操作が渡された場合、ArgumentNullExceptionがスローされることを確認します。
	/// </summary>
	[Fact]
	public async Task ExecuteAsync_NullOperation_ThrowsArgumentNullException() {
		// Arrange
		using var coordinator = new DatabaseWriteCoordinator();

		// Act & Assert
		await Should.ThrowAsync<ArgumentNullException>(() =>
			coordinator.ExecuteAsync((Func<CancellationToken, Task>)null!));
	}

	/// <summary>
	///     キャンセルトークンが尊重されることを確認します。
	/// </summary>
	[Fact]
	public async Task ExecuteAsync_CancelledToken_ThrowsOperationCanceledException() {
		// Arrange
		using var coordinator = new DatabaseWriteCoordinator();
		using var cts = new CancellationTokenSource();
		cts.Cancel();

		// Act & Assert
		await Should.ThrowAsync<OperationCanceledException>(() =>
			coordinator.ExecuteAsync(async ct => {
				await Task.Delay(1000, ct);
			}, cts.Token));
	}
}