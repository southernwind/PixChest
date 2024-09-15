using System.Collections.Generic;
using System.Threading.Tasks;

using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Models.Operators.Intarfaces;
using PixChest.Utils.Objects;

namespace PixChest.FileTypes.Base.Models;

public abstract class BaseFileModel(long id, string filePath, IFileOperator fileOperator) : ModelBase {
	protected IFileOperator FileOperator {
		get;
	} = fileOperator;

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
	public int Rate {
		get;
		set;
	}

	/// <summary>
	/// 使用回数
	/// </summary>
	public int UsageCount {
		get;
		set;
	}

	/// <summary>
	/// 説明
	/// </summary>
	public string Description {
		get;
		set;
	}

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

	public async Task UpdateRateAsync(int rate) {
		await this.FileOperator.UpdateRateAsync(this.Id, rate);
		this.Rate = rate;
	}

	public async Task IncrementUsageCountAsync() {
		await this.FileOperator.IncrementUsageCountAsync(this.Id);
		this.UsageCount++;
	}

	public async Task UpdateDescriptionAsync(string description) {
		await this.FileOperator.UpdateDescriptionAsync(this.Id, description);
		this.Description = description;
	}
}