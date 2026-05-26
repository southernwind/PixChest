using MediaDeck.Common.Base;
using MediaDeck.Common.Utilities;

namespace MediaDeck.Core.Services.Database;

/// <summary>
/// SQLiteへの書き込み処理をアプリケーション内で直列化するサービスです。
/// </summary>
[Inject(InjectServiceLifetime.Singleton, typeof(IDatabaseWriteCoordinator))]
public class DatabaseWriteCoordinator : ServiceBase, IDatabaseWriteCoordinator {
	private readonly AsyncLock _writeLock = new();

	/// <inheritdoc />
	public async Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default) {
		ArgumentNullException.ThrowIfNull(operation);

		using var _ = await this._writeLock.LockAsync(ct).ConfigureAwait(false);
		await operation(ct).ConfigureAwait(false);
	}

	/// <inheritdoc />
	public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken ct = default) {
		ArgumentNullException.ThrowIfNull(operation);

		using var _ = await this._writeLock.LockAsync(ct).ConfigureAwait(false);
		return await operation(ct).ConfigureAwait(false);
	}
}