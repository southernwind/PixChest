using MapControl;
using MediaDeck.Objects.Maps;
using MediaDeck.ViewModels.Panes.ViewerPanes;
using Microsoft.UI.Xaml;

namespace MediaDeck.Views.Panes.ViewerPanes;

public sealed partial class MapViewer {
	public MapViewer() {
		this.InitializeComponent();
	}

	private void Map_Loaded(object sender, RoutedEventArgs e) {
		if (this.ViewModel is not { }) {
			return;
		}
		this.UpdateMapControl();
	}

	private void Pin_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e) {
		if (this.ViewModel is not { } vm) {
			return;
		}
		var pinVm = (MapPinViewModel)((FrameworkElement)sender).DataContext;
		vm.MapViewerViewModel.SelectPin(pinVm);
	}

	private void UpdateMapControl() {
		this.Map.PointerWheelChanged += (_, _) => {
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
		};
	}
}