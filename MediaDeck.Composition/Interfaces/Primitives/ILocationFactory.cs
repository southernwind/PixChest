namespace MediaDeck.Composition.Interfaces.Primitives;

/// <summary>
/// ILocation のファクトリインターフェース
/// </summary>
public interface ILocationFactory {
	/// <summary>
	/// 座標情報を生成する
	/// </summary>
	public ILocation Create(double latitude, double longitude, double? altitude = null);
}