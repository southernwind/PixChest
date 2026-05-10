using GenJsonConfig.Attributes;

namespace MediaDeck.Composition.Interfaces.Files;

[GenerateJsonConfigDto]
public interface IFilterItemObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public string DisplayName {
		get;
	}
}