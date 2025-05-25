using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using PixChest.Composition.Bases;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.Files;
using PixChest.Models.Preferences.CustomConfigs;
using PixChest.Utils.Enums;
using PixChest.Utils.Objects;

namespace PixChest.FileTypes.Base.Models;

public abstract class BaseFileModel(long id, string filePath, IFileOperator fileOperator) : ModelBase, IFileModel {
	private static readonly ExecutionConfig executionConfig;
	static BaseFileModel() {
		executionConfig = Ioc.Default.GetRequiredService<ExecutionConfig>();
	}
	protected IFileOperator FileOperator {
		get;
	} = fileOperator;

	public abstract MediaType MediaType {
		get;
	}

	public long Id {
		get;
	} = id;

	public string FilePath {
		get;
	} = filePath;

	public string? ThumbnailFilePath {
		get;
		set;
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
	public List<TagModel> Tags {
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
	} = "";

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
					{ "ファイルサイズ",$"{StringUtility.LongToFileSize(this.FileSize)}" },
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

	public async Task ExecuteFileAsync() {
		var epo = executionConfig.ExecutionProgramObjects.FirstOrDefault(x => x.MediaType.Value == this.MediaType);
		if (epo is null) {
			var psi = new ProcessStartInfo {
				FileName = this.FilePath,
				UseShellExecute = true
			};
			_ = Process.Start(psi);
		} else {
			var psi = new ProcessStartInfo {
				FileName = epo.Path.Value,
				Arguments = string.Format(epo.Args.Value, this.FilePath),
				UseShellExecute = true
			};
			_ = Process.Start(psi);
		}

		await this.IncrementUsageCountAsync();
	}
}