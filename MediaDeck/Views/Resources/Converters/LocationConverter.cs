using MapControl;
using MediaDeck.Composition.Interfaces.Primitives;
using MediaDeck.Objects.Maps;
using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// <see cref="ILocation"/> と <see cref="Location"/> の相互変換を行うコンバーター
/// </summary>
public class LocationConverter : IValueConverter {
	/// <inheritdoc/>
	public object? Convert(object? value, Type targetType, object? parameter, string language) {
		if (value is ILocation location) {
			return new Location(location.Latitude, location.Longitude);
		}
		return null;
	}

	/// <inheritdoc/>
	public object? ConvertBack(object? value, Type targetType, object? parameter, string language) {
		if (value is Location mapLocation) {
			return new MediaDeckLocation(mapLocation.Latitude, mapLocation.Longitude);
		}
		return null;
	}
}