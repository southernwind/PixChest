using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Utils.Objects;

namespace PixChest.Models.Files.FileTypes.Base;

[AddTransient]
public class FileModel(long id, string filePath) : ModelBase {
	public long Id {
		get;
	} = id;

	public string FilePath {
		get;
	} = filePath;

	public string? ThumbnailFilePath {
		get;
		init;
	}

	public bool Exists {
		get;
		set;
	}

	/// <summary>
	/// 座標
	/// </summary>
	public GpsLocation? Location {
		get;
		set;
	}

	/// <summary>
	/// タグリスト
	/// </summary>
	public List<string> Tags {
		get;
		set;
	} = [];

	/// <summary>
	/// 解像度
	/// </summary>
	public ComparableSize? Resolution {
		get;
		set;
	}

	/// <summary>
	/// 評価
	/// </summary>
	public ReactiveProperty<int> Rate {
		get;
	} = new();

	/// <summary>
	/// 作成日時
	/// </summary>
	public DateTime CreationTime {
		get;
		set;
	}

	/// <summary>
	/// 編集日時
	/// </summary>
	public DateTime ModifiedTime {
		get;
		set;
	}

	/// <summary>
	/// 最終アクセス日時
	/// </summary>
	public DateTime LastAccessTime {
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
	/// プロパティ
	/// </summary>
	public virtual Attributes<string> Properties {
		get {
			return new Dictionary<string, string> {
					{ "作成日時",$"{this.CreationTime}" },
					{ "編集日時",$"{this.ModifiedTime}" },
					{ "最終アクセス日時",$"{this.LastAccessTime}" },
					{ "ファイルサイズ",$"{this.FileSize}" },
					{ "解像度" , $"{this.Resolution?.ToString()}" }
				}.ToAttributes();
		}
	}
}