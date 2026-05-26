namespace MediaDeck.Composition.Interfaces.Services;

/// <summary>
/// データベースへの書き込み処理をアプリケーション内で直列化するサービスです。
/// </summary>
public interface IDatabaseWriteCoordinator : IServiceBase {
	/// <summary>
	/// データベースへの書き込み処理を排他実行します。
	/// </summary>
	/// <param name="operation">排他実行する処理</param>
	/// <param name="ct">キャンセルトークン</param>
	public Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default);

	/// <summary>
	/// 戻り値を持つデータベースへの書き込み処理を排他実行します。
	/// </summary>
	/// <typeparam name="T">戻り値の型</typeparam>
	/// <param name="operation">排他実行する処理</param>
	/// <param name="ct">キャンセルトークン</param>
	/// <returns>排他実行した処理の戻り値</returns>
	public Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken ct = default);
}