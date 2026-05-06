using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.MediaItemTypes;
using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;

using Microsoft.Extensions.DependencyInjection;

using R3.JsonConfig.Attributes;

namespace MediaDeck.Composition.Stores.Config.Model;

[Inject(InjectServiceLifetime.Singleton)]
[GenerateR3JsonConfigDto]
public class ExecutionConfigModel {
	private readonly IServiceProvider _serviceProvider;
	private IMediaItemTypeService _mediaItemTypeService {
		get {
			return field ??= this._serviceProvider.GetRequiredService<IMediaItemTypeService>();
		}
	}

	public ExecutionConfigModel(IServiceProvider serviceProvider) {
		this._serviceProvider = serviceProvider;
	}

	/// <summary>
	/// 実行プログラム
	/// </summary>
	public ObservableList<IExecutionProgramObjectModel> ExecutionPrograms {
		get;
	} = [];

	public void AddExecutionProgram(MediaType mediaType) {
		var model = this._mediaItemTypeService.CreateExecutionProgramObjectModel(mediaType);
		// If this is the first program for the media type, mark it as default
		if (!this.ExecutionPrograms.Any(x => x.MediaType == mediaType)) {
			model.IsDefault.Value = true;
		}
		this.ExecutionPrograms.Add(model);
	}

	public void RemoveExecutionProgram(IExecutionProgramObjectModel program) {
		this.ExecutionPrograms.Remove(program);
	}

	/// <summary>
	/// 指定メディアタイプのデフォルト実行プログラムを取得
	/// </summary>
	public IExecutionProgramObjectModel? GetDefaultProgram(MediaType mediaType) {
		return this.ExecutionPrograms
			.Where(x => x.MediaType == mediaType)
			.FirstOrDefault(x => x.IsDefault.Value)
			?? this.ExecutionPrograms.FirstOrDefault(x => x.MediaType == mediaType);
	}

	/// <summary>
	/// 指定メディアタイプに紐付く全プログラムを取得
	/// </summary>
	public IReadOnlyList<IExecutionProgramObjectModel> GetPrograms(MediaType mediaType) {
		return this.ExecutionPrograms
			.Where(x => x.MediaType == mediaType)
			.ToList();
	}

	/// <summary>
	/// 指定されたプログラムを既定のツールとして設定し、同メディアタイプの他のプログラムの既定状態を解除する
	/// </summary>
	public void SetDefaultProgram(IExecutionProgramObjectModel defaultProgram) {
		if (defaultProgram == null)
			return;

		foreach (var program in this.ExecutionPrograms.Where(x => x.MediaType == defaultProgram.MediaType)) {
			if (program != defaultProgram) {
				program.IsDefault.Value = false;
			} else {
				program.IsDefault.Value = true;
			}
		}
	}
}