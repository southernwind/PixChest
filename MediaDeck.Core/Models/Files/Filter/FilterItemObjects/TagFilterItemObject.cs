using GenJsonConfig.Attributes;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces.Files;

namespace MediaDeck.Core.Models.Files.Filter.FilterItemObjects;

/// <summary>
/// タグフィルターアイテムオブジェクト
/// </summary>
[GenerateJsonConfigDto]
[JsonConfigDerivedType("tagFilter")]
[Inject(InjectServiceLifetime.Transient)]
public class TagFilterItemObject : IFilterItemObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public string DisplayName {
		get {
			return $"{this.TagName} tag {(this.SearchType == SearchTypeInclude.Include ? "included" : "not included")}";
		}
	}

	/// <summary>
	/// タグ名
	/// </summary>
	public string TagName {
		get {
			return field ?? throw new InvalidOperationException($"{nameof(this.TagName)} is not initialized.");
		}
		set {
			field = value;
		}
	}

	/// <summary>
	/// 検索タイプ
	/// </summary>
	public SearchTypeInclude SearchType {
		get;
		set;
	}

	public TagFilterItemObject() {
	}
}