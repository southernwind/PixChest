using PixChest.Composition.Bases;
using PixChest.Utils.Objects;

namespace PixChest.Models.Files;

[AddTransient]
public class FileModel(string filePath) : ModelBase {
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
	public Reactive.Bindings.ReactiveCollection<string> Tags {
		get;
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
}