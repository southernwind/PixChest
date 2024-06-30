using PixChest.Composition.Enum;
using PixChest.Composition.Objects;
using System.Threading;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using R3;

namespace PixChest.Composition.Bases;
public class ModelBase : ObservableObject, IModelBase {
	/// <summary>
	/// Dispose用Lockオブジェクト
	/// 処理を行っている途中でDisposeされるとマズイ場合、このオブジェクトでロックしておく。
	/// </summary>
	protected readonly DisposableLock DisposeLock = new(LockRecursionPolicy.SupportsRecursion);
	/// <summary>
	/// まとめてDispose
	/// </summary>
	private CompositeDisposable? _compositeDisposable;

	/// <summary>
	/// Dispose通知用Subject
	/// </summary>
	private readonly Subject<Unit> _onDisposed = new();

	/// <summary>
	/// Dispose済みか
	/// </summary>
	public DisposeState DisposeState {
		get;
		private set;
	}

	/// <summary>
	/// Dispose通知
	/// </summary>
	public Observable<Unit> OnDisposed {
		get {
			return this._onDisposed.AsObservable();
		}
	}

	/// <summary>
	/// まとめてDispose
	/// </summary>
	public CompositeDisposable CompositeDisposable {
		get {
			return this._compositeDisposable ??= [];
		}
	}
	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose() {
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose
	/// </summary>
	/// <param name="disposing">マネージドリソースの破棄を行うかどうか</param>
	protected virtual void Dispose(bool disposing) {
		lock (this.DisposeLock) {
			if (this.DisposeState != DisposeState.NotDisposed) {
				return;
			}
			using (this.DisposeLock.DisposableEnterWriteLock()) {
				if (this.DisposeState != DisposeState.NotDisposed) {
					return;
				}
				this.DisposeState = DisposeState.Disposing;
			}
			if (disposing) {
				this._onDisposed.OnNext(Unit.Default);
				this._compositeDisposable?.Dispose();
			}
			using (this.DisposeLock.DisposableEnterWriteLock()) {
				this.DisposeState = DisposeState.Disposed;
			}
			this.DisposeLock.Dispose();
		}
	}
}
