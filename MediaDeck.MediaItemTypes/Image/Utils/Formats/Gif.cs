using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Gif;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Gifメタデータ取得クラス
/// </summary>
public class Gif : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Gif(Stream stream) : base(stream) {
		this._reader = GifMetadataReader.ReadMetadata(stream);
		var d = this._reader.First(x => x is GifHeaderDirectory);
		this.Width = d.GetUInt16(GifHeaderDirectory.TagImageWidth);
		this.Height = d.GetUInt16(GifHeaderDirectory.TagImageHeight);
	}

	public Composition.Tables.Metadata.MediaMetadata CreateMetadataRecord() {
		var metadata = new Composition.Tables.Metadata.MediaMetadata();

		foreach (var directory in this._reader) {
			foreach (var tag in directory.Tags) {
				var value = tag.Description;
				if (value != null) {
					metadata.Entries.Add(new() { Key = $"{directory.Name}/{tag.Name}", Value = value });
				}
			}
		}

		return metadata;
	}
}