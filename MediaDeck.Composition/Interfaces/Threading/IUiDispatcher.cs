namespace MediaDeck.Composition.Interfaces.Threading;

/// <summary>
/// UIスレッド上で処理を実行するためのディスパッチャーインターフェース。
/// </summary>
public interface IUiDispatcher {
	/// <summary>
	/// 指定されたアクションを UI スレッド上で同期的に実行します。
	/// </summary>
	/// <param name="action">実行するアクション</param>
	public void Run(Action action);
}