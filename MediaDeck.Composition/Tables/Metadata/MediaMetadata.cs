namespace MediaDeck.Composition.Tables.Metadata;

/// <summary>
/// 統一メタデータクラス（JSONカラムとして格納）
/// すべてのメタデータをキー・バリューのリストとして保持する。
/// </summary>
public class MediaMetadata {
	/// <summary>
	/// メタデータエントリのリスト
	/// </summary>
	public List<MediaMetadataEntry> Entries {
		get;
		set;
	} = [];
}

/// <summary>
/// メタデータの個別エントリ（キー・バリュー）
/// </summary>
public class MediaMetadataEntry {
	/// <summary>
	/// キー（例: "Make", "ExposureTime", "Duration" など）
	/// </summary>
	public required string Key {
		get;
		set;
	}

	/// <summary>
	/// 値（文字列表現）
	/// </summary>
	public required string Value {
		get;
		set;
	}
}