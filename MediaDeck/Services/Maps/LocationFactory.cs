using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces.Primitives;
using MediaDeck.Objects.Maps;

namespace MediaDeck.Services.Maps;

/// <summary>
/// Location のファクトリ実装
/// </summary>
[Inject(InjectServiceLifetime.Singleton, typeof(ILocationFactory))]
public class LocationFactory : ServiceBase, ILocationFactory {
	/// <inheritdoc/>
	public ILocation Create(double latitude, double longitude, double? altitude = null) {
		return new MediaDeckLocation(latitude, longitude, altitude);
	}
}