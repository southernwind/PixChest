using System.Linq.Expressions;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;

namespace PixChest.Models.Files.SearchConditions;
public class TagSearchCondition: ISearchCondition {
	public TagSearchCondition(TagModel targetTag) {
		this.TargetTag = targetTag;
	}

	public TagModel TargetTag {
		get;
	}

	public string DisplayText {
		get {
			return $"TagName={this.TargetTag.TagName}";
		}
	}

	public Expression<Func<MediaFile, bool>>? WherePredicate {
		get {
			Expression<Func<MediaFile, bool>> exp1 =
				mediaFile => mediaFile.MediaFileTags.Select(x => x.TagId).Contains(this.TargetTag.TagId);
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
