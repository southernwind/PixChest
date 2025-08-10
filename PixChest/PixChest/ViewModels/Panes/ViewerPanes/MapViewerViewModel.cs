using System.Collections.Generic;
using MapControl;
using PixChest.Models.FileDetailManagers;
using PixChest.Models.Maps;

namespace PixChest.ViewModels.Panes.ViewerPanes;

[AddTransient]
public class MapViewerViewModel : ViewerPaneViewModelBase {
	private readonly MapModel _mapModel;
	public MapViewerViewModel(FilesManager filesManager,MapModel mapModel) : base ("Map", filesManager){
		this._mapModel = mapModel;
		this.MapPins = this._mapModel.MapPins.ToBindableReactiveProperty();
	}

	public BindableReactiveProperty<IEnumerable<MapPin>?> MapPins {
		get;
	}

	public void UpdateMapControl(Map map) {
		this._mapModel.UpdateMapControl(map);
	}
}
