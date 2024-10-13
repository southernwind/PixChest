
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;

namespace PixChest.Models.Files.SearchConditions;
public class WordSearchCondition: ISearchCondition {
	public WordSearchCondition(string word) {
		this.Word = word;
	}

	public string Word {
		get;
	}

	public string DisplayText {
		get {
			return $"Word={this.Word}";
		}
	}

	public Expression<Func<MediaFile, bool>>? WherePredicate {
		get {
			Expression<Func<MediaFile, bool>> exp1 =
			mediaFile =>
				EF.Functions.Like(mediaFile.FilePath, $"%{this.Word}%") ||
				EF.Functions.Like(mediaFile.Position!.DisplayName!, $"%{this.Word}%");
			var exp = exp1.Body;
			var visitor = new ParameterVisitor(exp1.Parameters);

			return Expression.Lambda<Func<MediaFile, bool>>(
				exp,
				visitor.Parameters);
		}
	}

	public Func<IFileModel, bool>? Filter {
		get;
	} = null;
}
