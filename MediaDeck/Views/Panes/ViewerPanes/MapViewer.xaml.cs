using System.Collections.Specialized;
using MapControl;
using MediaDeck.Objects.Maps;
using MediaDeck.ViewModels.Panes.ViewerPanes;
using Microsoft.UI.Xaml;

namespace MediaDeck.Views.Panes.ViewerPanes;

public sealed partial class MapViewer {
	private readonly Subject<Unit> PinUpdateRequest = new();
	private readonly CompositeDisposable _disposables = new();
	public MapViewer() {
		this.InitializeComponent();
		this.PinUpdateRequest
			.Debounce(TimeSpan.FromMilliseconds(300))
			.ObserveOnCurrentSynchronizationContext()
			.Subscribe(x => {
				if (this.Map is not { } map) {
					return;
				}
				if (this.ViewModel is not { }) {
					return;
				}
				var leftTop = map.ViewToLocation(new(0, 0));
				var rightBottom = map.ViewToLocation(new(map.ActualWidth, map.ActualHeight));

				var mapVm = this.ViewModel.MapViewerViewModel;
				mapVm.BoundsNorthWest.Value = new MediaDeckLocation(leftTop.Latitude, leftTop.Longitude);
				mapVm.BoundsSouthEast.Value = new MediaDeckLocation(rightBottom.Latitude, rightBottom.Longitude);

				mapVm.UpdateItemsForMapView(loc => {
					var viewPoint = map.LocationToView(new Location(loc.Latitude, loc.Longitude));
					return new System.Drawing.Point((int)viewPoint.X, (int)viewPoint.Y);
				}, 100);
			}).AddTo(this._disposables);
	}

	private void Map_Loaded(object sender, RoutedEventArgs e) {
		if (this.ViewModel is not { } vm) {
			return;
		}
		this.UpdateMapControl();

		// 初期状態で全アイテムが入るように調整
		// Files が空でない場合は即座に、空の場合は最初の更新を待つ
		if (vm.MapViewerViewModel.MediaContentLibraryViewModel.Files.Count > 0) {
			vm.MapViewerViewModel.FitToItems(this.Map.ActualWidth, this.Map.ActualHeight);
		}

		// アイテム変更時にもフィットさせる
		vm.MapViewerViewModel.MediaContentLibraryViewModel.Files.CollectionChanged += this.OnFilesCollectionChanged;
	}

	private void OnFilesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		if (this.ViewModel is not { } vm) {
			return;
		}
		this.DispatcherQueue.TryEnqueue(() => {
			this.PinUpdateRequest.OnNext(Unit.Default);
		});
	}

	private void Pin_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e) {
		if (this.ViewModel is not { } vm) {
			return;
		}
		var pinVm = (MapPinViewModel)((FrameworkElement)sender).DataContext;
		vm.MapViewerViewModel.SelectPin(pinVm);
	}

	private void UpdateMapControl() {
		// ビューポート変更時にアイテムを更新する
		this.Map.ViewportChanged += (_, _) => {
			this.PinUpdateRequest.OnNext(Unit.Default);
		};
	}

	~MapViewer() {
		this._disposables.Dispose();
	}
}