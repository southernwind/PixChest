using System.Linq.Expressions;
using GenJsonConfig.Attributes;
using MediaDeck.Composition.Tables;

namespace MediaDeck.Composition.Interfaces.Files;

[GenerateJsonConfigDto]
public interface ISearchCondition {
	public string DisplayText {
		get;
	}

	public Expression<Func<MediaItem, bool>>? WherePredicate {
		get;
	}

	public bool IsMatchForSuggest(string searchWord);
}