using PixChest.Database.Tables.Metadata;

namespace PixChest.Database.Tables {
	/// <summary>
	/// メディアファイルテーブル
	/// </summary>
	public class MediaFile {
		private string? _directoryPath;
		private string? _filePath;
		private ICollection<MediaFileTag>? _mediaFileTags;

		/// <summary>
		/// メディアファイルID
		/// </summary>
		public long MediaFileId {
			get;
			set;
		}

		/// <summary>
		/// ディレクトリパス
		/// </summary>
		public string DirectoryPath {
			get {
				return this._directoryPath ?? throw new InvalidOperationException();
			}
			set {
				this._directoryPath = value;
			}
		}

		/// <summary>
		/// ファイル名
		/// </summary>
		public string FilePath {
			get {
				return this._filePath ?? throw new InvalidOperationException();
			}
			set {
				this._filePath = value;
			}
		}

		/// <summary>
		/// サムネイルファイル名
		/// </summary>
		public string? ThumbnailFileName {
			get;
			set;
		}

		/// <summary>
		/// 緯度
		/// </summary>
		public double? Latitude {
			get;
			set;
		}

		/// <summary>
		/// 経度
		/// </summary>
		public double? Longitude {
			get;
			set;
		}

		/// <summary>
		/// 高度
		/// </summary>
		public double? Altitude {
			get;
			set;
		}

		/// <summary>
		/// ファイルサイズ
		/// </summary>
		public long FileSize {
			get;
			set;
		}

		/// <summary>
		/// 評価
		/// </summary>
		public int Rate {
			get;
			set;
		}

		/// <summary>
		/// 解像度 幅
		/// </summary>
		public int Width {
			get;
			set;
		}

		/// <summary>
		/// 解像度 高さ
		/// </summary>
		public int Height {
			get;
			set;
		}

		/// <summary>
		/// ファイルハッシュ
		/// </summary>
		public string? Hash {
			get;
			set;
		}

		/// <summary>
		/// 不正ファイルか否か
		/// </summary>
		public bool IsInvalid {
			get;
			set;
		}

		/// <summary>
		/// 自動生成サムネイルか否か
		/// </summary>
		public bool IsAutoGeneratedThumbnail {
			get;
			set;
		}

		/// <summary>
		/// タグリスト
		/// </summary>
		public virtual ICollection<MediaFileTag> MediaFileTags {
			get {
				return this._mediaFileTags ?? throw new InvalidOperationException();
			}
			set {
				this._mediaFileTags = value;
			}
		}

		/// <summary>
		/// 動画ファイル
		/// </summary>
		public virtual VideoFile? VideoFile {
			get;
			set;
		}

		/// <summary>
		/// 画像ファイル
		/// </summary>
		public virtual ImageFile? ImageFile {
			get;
			set;
		}

		/// <summary>
		/// 位置情報
		/// </summary>
		public virtual Position? Position {
			get;
			set;
		}

		/// <summary>
		/// Jpegメタデータ
		/// </summary>
		public virtual Jpeg? Jpeg {
			get;
			set;
		}

		/// <summary>
		/// Pngメタデータ
		/// </summary>
		public virtual Png? Png {
			get;
			set;
		}

		/// <summary>
		/// Bmpメタデータ
		/// </summary>
		public virtual Bmp? Bmp {
			get;
			set;
		}

		/// <summary>
		/// Gifメタデータ
		/// </summary>
		public virtual Gif? Gif {
			get;
			set;
		}

		/// <summary>
		/// Heif メタデータ
		/// </summary>
		public virtual Heif? Heif {
			get;
			set;
		}
	}
}