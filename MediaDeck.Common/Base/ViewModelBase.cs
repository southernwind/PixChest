using MediaDeck.Composition.Interfaces;

namespace MediaDeck.Common.Base;

/// <summary>
/// ViewModel基底クラス
/// </summary>
public class ViewModelBase : DisposableBase, IViewModelBase {
	private readonly Subject<Unit> _requestClose = new();

	/// <summary>
	/// ウィンドウを閉じるリクエスト
	/// </summary>
	public Observable<Unit> RequestClose {
		get {
			return this._requestClose;
		}
	}

	/// <summary>
	/// ウィンドウを閉じるリクエストを発行します
	/// </summary>
	protected void Close() {
		this._requestClose.OnNext(Unit.Default);
	}

	protected override void Dispose(bool disposing) {
		if (disposing) {
			this._requestClose.Dispose();
		}
		base.Dispose(disposing);
	}
}