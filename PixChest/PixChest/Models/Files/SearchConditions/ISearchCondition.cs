using System.Linq.Expressions;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;

namespace PixChest.Models.Files.SearchConditions;
public interface ISearchCondition {
	public string DisplayText {
		get;
	}

	public Expression<Func<MediaFile, bool>>? WherePredicate {
		get;
	}

	public Func<IFileModel, bool>? Filter {
		get;
	}

	public bool IsMatchForSuggest(string searchWord);
}
