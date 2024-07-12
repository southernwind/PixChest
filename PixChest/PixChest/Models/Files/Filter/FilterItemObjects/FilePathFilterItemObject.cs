using PixChest.Utils.Enums;

namespace PixChest.Models.Files.Filter.FilterItemObjects;
/// <summary>
/// ファイルパスフィルターアイテムオブジェクト
/// </summary>
public class FilePathFilterItemObject : IFilterItemObject {
	/// <summary>
	/// 表示名
	/// </summary>
	public string DisplayName {
		get {
			return $"{this.Text} in file path {(this.SearchType == SearchTypeInclude.Include ? "includes" : "does not include")}";
		}
	}
	/// <summary>
	/// パスに含まれる文字列
	/// </summary>
	public string Text {
		get;
		set;
	}

	/// <summary>
	/// 検索タイプ
	/// </summary>
	public SearchTypeInclude SearchType {
		get;
		set;
	}

	[Obsolete("for serialize")]
	public FilePathFilterItemObject() {
		this.Text = null!;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="text">パスに含まれる文字列</param>
	/// <param name="searchType">検索タイプ</param>
	public FilePathFilterItemObject(string text, SearchTypeInclude searchType) {
		if (text == null || !Enum.IsDefined(typeof(SearchTypeInclude), searchType)) {
			throw new ArgumentException();
		}
		this.Text = text;
		this.SearchType = searchType;
	}
}
