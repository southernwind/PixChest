using System.IO;

using MediaDeck.Composition.Tables.Metadata;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Tiffメタデータ取得クラス
/// </summary>
public class Tiff : ImageBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Tiff(Stream stream) : base(stream) { }

	public override MediaMetadata CreateMetadata() {
		return new MediaMetadata();
	}
}