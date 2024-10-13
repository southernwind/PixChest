using PixChest.Database.Tables;

namespace PixChest.Models.Files; 
public class TagModel {
	public TagModel(Tag tag) {
		this.TagId = tag.TagId;
		this.TagCategoryId = tag.TagCategoryId;
		this.TagName = tag.TagName;
		this.Detail = tag.Detail;
	}

	/// <summary>
	/// タグID
	/// </summary>
	public int TagId {
		get;
		set;
	}

	/// <summary>
	/// タグ分類
	/// </summary>
	public int TagCategoryId {
		get;
		set;
	}

	/// <summary>
	/// タグ名
	/// </summary>
	public string TagName {
		get;
		set;
	}

	/// <summary>
	/// タグ説明
	/// </summary>
	public string Detail {
		get;
		set;
	}
}
