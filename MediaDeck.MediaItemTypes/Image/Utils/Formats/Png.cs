using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Png;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Pngメタデータ取得クラス
/// </summary>
public class Png : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Png(Stream stream) : base(stream) {
		this._reader = PngMetadataReader.ReadMetadata(stream);
		var d = this._reader.First(x => x is PngDirectory);
		this.Width = d.GetUInt16(PngDirectory.TagImageWidth);
		this.Height = d.GetUInt16(PngDirectory.TagImageHeight);
	}

	public override Composition.Tables.Metadata.MediaMetadata CreateMetadata() {
		return base.CreateMetadata(this._reader);
	}
}