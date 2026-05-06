using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.WebP;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Riffメタデータ取得クラス
/// </summary>
public class Riff : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Riff(Stream stream) : base(stream) {
		this._reader = WebPMetadataReader.ReadMetadata(stream);
		var d = this._reader.First(x => x is WebPDirectory);
		this.Width = d.GetUInt16(WebPDirectory.TagImageWidth);
		this.Height = d.GetUInt16(WebPDirectory.TagImageHeight);
	}

	public override Composition.Tables.Metadata.MediaMetadata CreateMetadata() {
		return base.CreateMetadata(this._reader);
	}
}