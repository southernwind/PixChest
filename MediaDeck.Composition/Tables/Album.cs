namespace MediaDeck.Composition.Tables;

/// <summary>
/// アルバムテーブル
/// </summary>
public class Album {
	/// <summary>
	/// アルバムID
	/// </summary>
	public int AlbumId {
		get;
		set;
	}

	/// <summary>
	/// アルバムパス（仮想階層、'/' 区切り）
	/// 例: 旅行/2024/北海道
	/// </summary>
	public required string Path {
		get;
		set;
	}

	/// <summary>
	/// 作成日時
	/// </summary>
	public DateTime CreatedTime {
		get;
		set;
	} = DateTime.UtcNow;

	/// <summary>
	/// 最終アクセス日時（最近使用したアルバム表示用）
	/// </summary>
	public DateTime LastAccessedTime {
		get;
		set;
	} = DateTime.UtcNow;

	/// <summary>
	/// 紐付くメディアアイテム
	/// </summary>
	public virtual ICollection<MediaItemAlbum> MediaItemAlbums {
		get {
			return field ?? throw new InvalidOperationException();
		}
		set;
	}
}