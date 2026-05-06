using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Photoshop;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Psdメタデータ取得クラス
/// </summary>
public class Psd : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;
	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Psd(Stream stream) : base(stream) {
		this._reader = PsdMetadataReader.ReadMetadata(stream);
		var d = this._reader.First(x => x is PsdHeaderDirectory);
		this.Width = d.GetUInt16(PsdHeaderDirectory.TagImageWidth);
		this.Height = d.GetUInt16(PsdHeaderDirectory.TagImageHeight);
	}

	public override Composition.Tables.Metadata.MediaMetadata CreateMetadata() {
		return base.CreateMetadata(this._reader);
	}
}