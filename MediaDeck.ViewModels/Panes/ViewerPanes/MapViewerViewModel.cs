using MediaDeck.Composition.Interfaces.MediaItemTypes.ViewModels;
using MediaDeck.Composition.Interfaces.Primitives;
using MediaDeck.Composition.Stores.State.Model;
using MediaDeck.Core.Models.Files;
using MediaDeck.Core.Models.Maps;

namespace MediaDeck.ViewModels.Panes.ViewerPanes;

[Inject(InjectServiceLifetime.Transient)]
public class MapViewerViewModel : ViewerPaneViewModelBase {
	private readonly MediaContentLibraryViewModel _mediaContentLibraryViewModel;
	private readonly ILocationFactory _locationFactory;

	public MapViewerViewModel(MediaContentLibraryViewModel mediaContentLibraryViewModel, FilesManager filesManager, ILocationFactory locationFactory)
		: base(ViewerType.Map, "Map", "\uE800", filesManager) {
		this._mediaContentLibraryViewModel = mediaContentLibraryViewModel;
		this._locationFactory = locationFactory;

		this.Center = new BindableReactiveProperty<ILocation>(this._locationFactory.Create(35, 135));

		// 選択ファイルの変化を監視してピンの PinState を更新
		mediaContentLibraryViewModel.SelectedFiles
			.Subscribe(selectedFiles => this.UpdatePinStates(selectedFiles))
			.AddTo(this.CompositeDisposable);
	}

	public MediaContentLibraryViewModel MediaContentLibraryViewModel {
		get {
			return this._mediaContentLibraryViewModel;
		}
	}

	/// <summary>
	/// 地図上のピンリスト
	/// </summary>
	public BindableReactiveProperty<IEnumerable<MapPinViewModel>?> MapPins {
		get;
	} = new();

	/// <summary>
	/// 地図の中心座標
	/// </summary>
	public BindableReactiveProperty<ILocation> Center {
		get;
	}

	/// <summary>
	/// 地図の表示範囲（緯度経度）
	/// </summary>
	public BindableReactiveProperty<ILocation?> BoundsNorthWest {
		get;
	} = new();

	public BindableReactiveProperty<ILocation?> BoundsSouthEast {
		get;
	} = new();

	/// <summary>
	/// ピンが選択されたときに呼び出す。対応するメディアを SelectedFiles に設定し、カメラ位置を更新する。
	/// </summary>
	public void SelectPin(MapPinViewModel pinVm) {
		if (pinVm is null) {
			return;
		}
		// ピンに含まれるメディアをすべて選択状態にする
		this._mediaContentLibraryViewModel.SelectedFiles.Value = pinVm.Items.ToArray();
	}

	/// <summary>
	/// 選択状態に基づいてピンの PinState を更新
	/// </summary>
	private void UpdatePinStates(IMediaItemViewModel[]? selectedFiles) {
		if (this.MapPins.Value is not { } pins) {
			return;
		}
		var selectedSet = selectedFiles?.ToHashSet() ?? [];
		foreach (var pin in pins) {
			var pinItems = pin.Items;
			var selectedCount = pinItems.Count(m => selectedSet.Contains(m));
			pin.PinState.Value = selectedCount switch {
				0 => PinState.Unselected,
				_ when selectedCount == pinItems.Count => PinState.Selected,
				_ => PinState.Indeterminate
			};
		}
	}

	/// <summary>
	/// 表示範囲を更新し、範囲内メディアのみにピンを再生成する。
	/// </summary>
	public void UpdateItemsForMapView(Func<ILocation, System.Drawing.Point> locationToPoint, int pinSize) {
		if (this.BoundsNorthWest.Value is not { } nw || this.BoundsSouthEast.Value is not { } se) {
			return;
		}

		var list = new List<MapPinViewModel>();

		foreach (var item in this._mediaContentLibraryViewModel.Files) {
			if (item.Location is not { } location) {
				continue;
			}

			// 範囲外チェック
			if (
				nw.Latitude < location.Latitude ||
				se.Latitude > location.Latitude ||
				nw.Longitude > location.Longitude ||
				se.Longitude < location.Longitude
			) {
				continue;
			}

			var viewPoint = locationToPoint(location);
			var rect = new Rectangle(viewPoint, new System.Drawing.Size(pinSize, pinSize));

			var cores = list.Where(x => rect.IntersectsWith(x.CoreRectangle)).ToList();
			if (cores.Count == 0) {
				list.Add(new MapPinViewModel(item, rect));
			} else {
				var target = cores.OrderBy(x => rect.DistanceTo(x.CoreRectangle)).First();
				target.AddItem(item);
			}
		}

		// 古いピンを Dispose する
		if (this.MapPins.Value is { } oldPins) {
			foreach (var oldPin in oldPins) {
				oldPin.Dispose();
			}
		}

		this.MapPins.Value = list.ToArray();
		this.UpdatePinStates(this._mediaContentLibraryViewModel.SelectedFiles.Value);
	}
}