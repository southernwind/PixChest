namespace MediaDeck.Common.Utilities;

/// <summary>
/// 非同期で動作する排他制御のためのロッククラスです。
/// using ステートメントを用いた IDisposable パターンによる解放をサポートします。
/// </summary>
public sealed class AsyncLock {
	private readonly SemaphoreSlim _semaphore = new(1, 1);
	private readonly Task<IDisposable> _releaser;

	/// <summary>
	/// <see cref="AsyncLock"/> クラスの新しいインスタンスを初期化します。
	/// </summary>
	public AsyncLock() {
		this._releaser = Task.FromResult((IDisposable)new Releaser(this));
	}

	/// <summary>
	/// 非同期ロックを取得します。
	/// </summary>
	/// <param name="cancellationToken">キャンセル トークン。</param>
	/// <returns>ロックを解放するための <see cref="IDisposable"/> を表すタスク。</returns>
	public Task<IDisposable> LockAsync(CancellationToken cancellationToken = default) {
		var wait = this._semaphore.WaitAsync(cancellationToken);
		return wait.IsCompleted ?
			this._releaser :
			wait.ContinueWith((_, state) => (IDisposable)state!,
				this._releaser.Result, cancellationToken,
				TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)!;
	}

	/// <summary>
	/// ロック解放用の IDisposable 実装クラスです。
	/// </summary>
	private sealed class Releaser : IDisposable {
		private readonly AsyncLock _toRelease;

		internal Releaser(AsyncLock toRelease) {
			this._toRelease = toRelease;
		}

		/// <summary>
		/// セマフォを解放します。
		/// </summary>
		public void Dispose() {
			this._toRelease._semaphore.Release();
		}
	}
}