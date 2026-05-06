using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Jpeg;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Jpegメタデータ取得クラス
/// </summary>
public class Jpeg : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Jpeg(Stream stream) : base(stream) {
		this._reader = JpegMetadataReader.ReadMetadata(stream);
		var d = this._reader.First(x => x is JpegDirectory);
		var gps = this._reader.FirstOrDefault(x => x is GpsDirectory);
		var ifd0 = this._reader.FirstOrDefault(x => x is ExifDirectoryBase);
		this.Width = d.GetUInt16(JpegDirectory.TagImageWidth);
		this.Height = d.GetUInt16(JpegDirectory.TagImageHeight);

		if (ifd0 != null && ifd0.TryGetUInt16(ExifDirectoryBase.TagOrientation, out var orientation)) {
			this.Orientation = orientation;
		}

		if (gps != null) {
			this.Latitude = gps.GetRationalArray(GpsDirectory.TagLatitude);
			this.Longitude = gps.GetRationalArray(GpsDirectory.TagLongitude);
			this.LatitudeRef = gps.GetString(GpsDirectory.TagLatitudeRef);
			this.LongitudeRef = gps.GetString(GpsDirectory.TagLongitudeRef);
			if (gps.TryGetRational(GpsDirectory.TagAltitude, out var r)) {
				this.Altitude = r;
			}
			if (gps.TryGetByte(GpsDirectory.TagAltitudeRef, out var b)) {
				this.AltitudeRef = b;
			}
		}
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