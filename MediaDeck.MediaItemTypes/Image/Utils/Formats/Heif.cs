using System.Collections.Generic;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Heif;

namespace MediaDeck.MediaItemTypes.Image.Utils.Formats;

/// <summary>
/// Heifメタデータ取得クラス
/// </summary>
public class Heif : ImageBase {
	private readonly IReadOnlyList<MetadataExtractor.Directory> _reader;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="stream">画像ファイルストリーム</param>
	public Heif(Stream stream) : base(stream) {
		this._reader = HeifMetadataReader.ReadMetadata(stream);
		var d = this._reader.OfType<HeicImagePropertiesDirectory>().First();
		var gps = this._reader.FirstOrDefault(x => x is GpsDirectory);
		var ifd0 = this._reader.FirstOrDefault(x => x is ExifDirectoryBase);
		this.Width = d.GetUInt16(HeicImagePropertiesDirectory.TagImageWidth);
		this.Height = d.GetUInt16(HeicImagePropertiesDirectory.TagImageHeight);

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

	public override Composition.Tables.Metadata.MediaMetadata CreateMetadata() {
		return base.CreateMetadata(this._reader);
	}
}