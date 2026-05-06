using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Bmp;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Bmpメタデータ取得クラス
/// </summary>
public class Bmp : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Bmp(Stream stream) : base(stream) {
		this._reader = BmpMetadataReader.ReadMetadata(stream);
		var d = this._reader.First(x => x is BmpHeaderDirectory);
		this.Width = d.GetUInt16(BmpHeaderDirectory.TagImageWidth);
		this.Height = d.GetUInt16(BmpHeaderDirectory.TagImageHeight);
	}

	public override Composition.Tables.Metadata.MediaMetadata CreateMetadata() {
		return base.CreateMetadata(this._reader);
	}
}