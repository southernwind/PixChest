using System.Linq.Expressions;

using PixChest.Composition.Bases;
using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;

namespace PixChest.Models.Files.Filter;
/// <summary>
/// フィルター条件
/// </summary>
/// <remarks>
/// <see cref="FilterItemCreators.IFilterItemCreator"/>から生成する。
/// </remarks>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="condition">フィルタリング条件</param>
/// <param name="conditionForModel">モデル用フィルタリング条件</param>
public class FilterItem(Expression<Func<MediaFile, bool>> condition, Func<IFileModel, bool> conditionForModel, bool includeSql) : ModelBase {
	/// <summary>
	/// フィルタリング条件
	/// </summary>
	public Expression<Func<MediaFile, bool>> Condition {
		get;
	} = condition;

	/// <summary>
	/// モデル用フィルタリング条件
	/// </summary>
	public Func<IFileModel, bool> ConditionForModel {
		get;
	} = conditionForModel;

	/// <summary>
	/// SQLに含めるか否か
	/// SQLに含めない場合、クエリ結果に対してC#上でフィルタリングを行う
	/// </summary>
	public bool IncludeSql {
		get;
	} = includeSql;

	public override string ToString() {
		return $"<[{base.ToString()}] {this.Condition}>";
	}
}
