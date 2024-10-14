using PixChest.Database.Tables;
using System.IO;
using PixChest.Models.Preferences;
using FFMpegCore;
using System.Text.RegularExpressions;
using PixChest.Database.Tables.Metadata;
using PixChest.Utils.Enums;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Drawing;
using PixChest.FileTypes.Base.Models;
using PixChest.FileTypes.Base.Models.Interfaces;
using System.Threading.Tasks;
using PixChest.Utils.Constants;

namespace PixChest.FileTypes.Video.Models;
[AddTransient]
public partial class VideoFileOperator : BaseFileOperator {
	private readonly Config _config;

	public override MediaType TargetMediaType {
		get;
	} = MediaType.Video;

	private static readonly string[] locationTagNames = [
			"TAG:location",
			"TAG:com.apple.quicktime.location.ISO6709"
		];

	public VideoFileOperator() {
		this._config = Ioc.Default.GetRequiredService<Config>();
	}

	public override async Task RegisterFileAsync(string filePath) {
		using var lockObject = await LockObjectConstants.DbLock.LockAsync();
		using var transaction = await this._db.Database.BeginTransactionAsync();
		var isExists = await this._db.MediaFiles.AnyAsync(x => x.FilePath == filePath);
		if (isExists) {
			return;
		}
		var metadata = FFProbe.Analyse(filePath);

		var thumbPath = FilePathUtility.GetThumbnailRelativeFilePath(filePath);
		try {
			if (metadata.PrimaryVideoStream is not { } videoStream) {
				throw new Exception("PrimaryVideoStream is null");
			}
			var image = this.CreateThumbnail(filePath, videoStream.Width, videoStream.Height, 300, 300, metadata.Duration / 5);
			new FileInfo(thumbPath).Directory?.Create();
			File.WriteAllBytes(thumbPath, image);
		} catch (Exception) {
			thumbPath = null;
		}

		var fileInfo = new FileInfo(filePath);
		var location = GetLocation(metadata);

		var mf = new MediaFile {
			DirectoryPath = Path.GetDirectoryName(filePath)!,
			FilePath = filePath,
			ThumbnailFileName = thumbPath,
			Rate = -1,
			Description = "",
			IsAutoGeneratedThumbnail = true,
			FileSize = fileInfo.Exists ? fileInfo.Length : 0,
			CreationTime = fileInfo.Exists ? fileInfo.CreationTime : DateTime.MinValue,
			ModifiedTime = fileInfo.Exists ? fileInfo.LastWriteTime : DateTime.MinValue,
			LastAccessTime = fileInfo.Exists ? fileInfo.LastAccessTime : DateTime.MinValue,
			IsExists = fileInfo.Exists,
			Latitude = location?.Latitude,
			Longitude = location?.Longitude,
			Altitude = location?.Altitude,
			Width = metadata.PrimaryVideoStream?.Width ?? 0,
			Height = metadata.PrimaryVideoStream?.Height ?? 0,
			VideoFile = new() {
				Duration = metadata.PrimaryVideoStream?.Duration.TotalSeconds,
				Rotation = metadata.PrimaryVideoStream?.Rotation,
				VideoMetadataValues = metadata.PrimaryVideoStream?.Tags?.Select(x => new VideoMetadataValue() { Key = x.Key, Value = x.Value }).ToList() ?? []
			}
		};

		await this._db.MediaFiles.AddAsync(mf);
		await this._db.SaveChangesAsync();
		await transaction.CommitAsync();
	}
	/// <summary>
	/// サムネイル作成
	/// </summary>
	/// <param name="fileModel">動画ファイル</param>
	/// <param name="width">サムネイル幅</param>
	/// <param name="height">サムネイル高さ</param>
	/// <param name="time">時間指定</param>
	/// <returns>作成されたサムネイルファイル名</returns>
	public byte[] CreateThumbnail(IFileModel fileModel, int width, int height, TimeSpan time) {


		var metadata = FFProbe.Analyse(fileModel.FilePath);
		if (metadata.PrimaryVideoStream is not { } videoStream) {
			throw new Exception("PrimaryVideoStream is null");
		}
		return this.CreateThumbnail(fileModel.FilePath, videoStream.Width, videoStream.Height, width, height, time);
	}

	/// <summary>
	/// サムネイル作成
	/// </summary>
	/// <param name="filePath">動画ファイルパス</param>
	/// <param name="originalWidth">元動画幅</param>
	/// <param name="originalHeight">元動画高さ</param>
	/// <param name="width">サムネイル幅</param>
	/// <param name="height">サムネイル高さ</param>
	/// <param name="time">時間指定</param>
	/// <returns>作成されたサムネイルファイル名</returns>
	public byte[] CreateThumbnail(string filePath, int originalWidth, int originalHeight, int width, int height, TimeSpan time) {
		if (originalWidth / width > originalHeight / height) {
			height = -1;
		} else {
			width = -1;
		}

		var uuid = Guid.NewGuid();
		var temporaryThumbPath = Path.Combine(this._config.PathConfig.TemporaryFolderPath.Value, $"{uuid}.png");
		FFMpeg.Snapshot(filePath, temporaryThumbPath, new Size(width, height), time);

		var binary = File.ReadAllBytes(temporaryThumbPath);
		File.Delete(temporaryThumbPath);
		return binary;
	}

	/// <summary>
	/// 位置情報取得
	/// </summary>
	/// <param name="mediaAnalysis">解析結果</param>
	/// <returns>位置情報</returns>
	private static (double Latitude, double Longitude, double? Altitude)? GetLocation(IMediaAnalysis mediaAnalysis) {

		var locationTag = mediaAnalysis.PrimaryVideoStream?.Tags?.FirstOrDefault(x => locationTagNames.Contains(x.Key)).Value;

		if (locationTag is null) {
			return null;
		}

		var match = LocationRegex().Match(locationTag);
		if (!match.Success) {
			return null;
		}
		var lat = match.Result("${lat}");
		var lon = match.Result("${lon}");
		var alt = match.Result("${alt}");

		if (alt != "") {
			return new(double.Parse(lat), double.Parse(lon), double.Parse(alt));
		} else {
			return new(double.Parse(lat), double.Parse(lon), null);
		}
	}

	[GeneratedRegex(@"^(?<lat>[\+-]\d+\.\d+?)(?<lon>[\+-]\d+\.\d+?)(?<alt>[\+-]\d+\.\d+?)?/$")]
	private static partial Regex LocationRegex();
}
