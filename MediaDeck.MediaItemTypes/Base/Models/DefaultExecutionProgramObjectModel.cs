using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.MediaItemTypes.Models;
using R3.JsonConfig.Attributes;

namespace MediaDeck.MediaItemTypes.Base.Models;

/// <summary>
/// 標準的な外部プログラム実行設定（パスと引数）を保持するオブジェクト。
/// </summary>
[GenerateR3JsonConfigDto]
[JsonConfigDerivedType("default")]
[Inject(InjectServiceLifetime.Transient)]
public class DefaultExecutionProgramObjectModel : IExecutionProgramObjectModel {
	public MediaType MediaType {
		get;
		set;
	}

	// ツール名（表示用）
	public ReactiveProperty<string> Name { get; } = new("");

	// 既定ツールフラグ
	public ReactiveProperty<bool> IsDefault { get; } = new(false);

	public ReactiveProperty<string> Path { get; } = new();

	public ReactiveProperty<string> Args { get; } = new();
}