using System.IO;

using MediaDeck.Composition.Tables.Metadata;

using MetadataExtractor;
using MetadataExtractor.Formats.Pcx;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Pcxメタデータ取得クラス
/// </summary>
public class Pcx : ImageBase {
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Pcx(Stream stream) : base(stream) {
		var d = PcxMetadataReader.ReadMetadata(stream);
		var xStart = d.GetUInt16(PcxDirectory.TagXMin);
		var xEnd = d.GetUInt16(PcxDirectory.TagXMax);
		var yStart = d.GetUInt16(PcxDirectory.TagYMin);
		var yEnd = d.GetUInt16(PcxDirectory.TagYMax);
		this.Width = xEnd - xStart + 1;
		this.Height = yEnd - yStart + 1;
	}

	public override MediaMetadata CreateMetadata() {
		return new MediaMetadata();
	}
}