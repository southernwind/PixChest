using GenJsonConfig.Attributes;
using MediaDeck.Composition.Interfaces.Files;

namespace MediaDeck.Core.Models.Files.Filter.FilterItemObjects;

/// <summary>
/// ファイルタイプフィルターアイテムオブジェクト
/// </summary>
[GenerateJsonConfigDto]
[JsonConfigDerivedType("mediaType")]
[Inject(InjectServiceLifetime.Transient)]

public class MediaTypeFilterItemObject : IFilterItemObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public string DisplayName {
		get {
			if (this.IsVideo) {
				return "Video file";
			} else {
				return "Image file";
			}
		}
	}

	/// <summary>
	/// 動画ファイルか否か
	/// </summary>
	public bool IsVideo {
		get;
		set;
	}

	public MediaTypeFilterItemObject() {
	}
}