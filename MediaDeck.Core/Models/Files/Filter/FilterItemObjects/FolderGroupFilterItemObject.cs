using GenJsonConfig.Attributes;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.Files;

namespace MediaDeck.Core.Models.Files.Filter.FilterItemObjects;

/// <summary>
/// フォルダグループフィルターアイテムオブジェクト
/// </summary>
[GenerateJsonConfigDto]
[JsonConfigDerivedType("folderGroupFilter")]
[Inject(InjectServiceLifetime.Transient)]
public class FolderGroupFilterItemObject : IFilterItemObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public string DisplayName {
		get {
			return $"{(this.SearchType == SearchTypeInclude.Include ? "Include" : "Exclude")} files in folder group";
		}
	}

	/// <summary>
	/// 検索タイプ
	/// </summary>
	public SearchTypeInclude SearchType {
		get;
		set;
	}

	public FolderGroupFilterItemObject() {
	}
}