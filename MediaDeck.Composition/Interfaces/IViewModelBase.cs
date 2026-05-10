namespace MediaDeck.Composition.Interfaces;

/// <summary>
/// ViewModel基底インターフェース
/// </summary>
public interface IViewModelBase : IDisposableBase {
	/// <summary>
	/// ウィンドウを閉じるリクエスト
	/// </summary>
	public Observable<Unit> RequestClose {
		get;
	}
}