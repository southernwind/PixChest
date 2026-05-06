using System.IO;
using System.Threading.Tasks;

using MediaDeck.Common.Utilities;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.MediaItemTypes;
using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Tables;

namespace MediaDeck.MediaItemTypes.Base;

public abstract class BaseMediaItemTypeProvider : IMediaItemTypeProvider {
	protected readonly ConfigModel _config;

	public BaseMediaItemTypeProvider(ConfigModel config, MediaType mediaType) {
		this._config = config;
		this.MediaType = mediaType;
	}

	public MediaType MediaType {
		get;
	}

	public abstract IQueryable<MediaItem> IncludeTables(IQueryable<MediaItem> MediaItems);

	public virtual bool IsTargetPath(string path) {
		var extension = Path.GetExtension(path);
		if (string.IsNullOrWhiteSpace(extension)) {
			return false;
		}

		return this._config.ScanConfig.TargetExtensions
			.Any(x =>
				x.MediaType.Value == this.MediaType &&
				x.Extension.Value.Equals(extension, StringComparison.CurrentCultureIgnoreCase));
	}

	public virtual MediaItemPathStatus GetPathStatus(string path) {
		var fileInfo = new FileInfo(path);
		if (!fileInfo.Exists) {
			return new(false, 0, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
		}

		return new(true, fileInfo.Length, fileInfo.CreationTime, fileInfo.LastWriteTime, fileInfo.LastAccessTime);
	}

	/// <summary>
	/// 指定されたファイルパスに対してこのメディアタイプ固有の実行処理を行う。
	/// デフォルト実装では、ExecutionConfig の外部プログラム設定に従って実行する。
	/// 各メディアタイプで独自の実行ロジックが必要な場合はオーバーライドする。
	/// </summary>
	/// <param name="filePath">実行対象のファイルパス</param>
	/// <param name="scopedServiceProvider">実行するタブのスコープを切ったサービスプロバイダー</param>
	/// <returns>非同期タスク</returns>
	public virtual Task ExecuteAsync(string filePath, IServiceProvider scopedServiceProvider, IExecutionProgramObjectModel? program = null) {
		program ??= this._config.ExecutionConfig.GetDefaultProgram(this.MediaType);

		if (program is not Models.DefaultExecutionProgramObjectModel defaultProgram) {
			ShellUtility.ShellExecute(filePath);
		} else {
			var arguments = ArgumentPlaceholderResolver.Resolve(defaultProgram.Args.Value, filePath);
			ShellUtility.ShellExecute(defaultProgram.Path.Value, arguments);
		}
		return Task.CompletedTask;
	}
}