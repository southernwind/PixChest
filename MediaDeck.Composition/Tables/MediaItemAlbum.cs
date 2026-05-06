namespace MediaDeck.Composition.Tables;

/// <summary>
/// メディアアイテム・アルバム中間テーブル
/// </summary>
public class MediaItemAlbum {
	/// <summary>
	/// メディアアイテムID
	/// </summary>
	public long MediaItemId {
		get;
		set;
	}

	/// <summary>
	/// メディアアイテム
	/// </summary>
	public MediaItem MediaItem {
		get {
			return field ?? throw new InvalidOperationException();
		}
		set;
	}

	/// <summary>
	/// アルバムID
	/// </summary>
	public int AlbumId {
		get;
		set;
	}

	/// <summary>
	/// アルバム
	/// </summary>
	public Album Album {
		get {
			return field ?? throw new InvalidOperationException();
		}
		set;
	}

	/// <summary>
	/// 追加日時
	/// </summary>
	public DateTime AddedTime {
		get;
		set;
	} = DateTime.UtcNow;
}