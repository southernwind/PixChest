using System.Collections.Generic;

using PixChest.Database.Tables;

namespace PixChest.Models.Files; 
public class TagModel {
	public TagModel(Tag tag) {
		this.TagId = tag.TagId;
		this.TagCategoryId = tag.TagCategoryId;
		this.TagCategory = new TagCategoryModel(tag.TagCategory);
		this.TagName = tag.TagName;
		this.Detail = tag.Detail;
		this.Romaji = tag.TagName.KatakanaToHiragana().HiraganaToRomaji();
		this.TagAliases = [.. tag.TagAliases.Select(x => new TagAliasModel(x))];
	}

	/// <summary>
	/// タグID
	/// </summary>
	public int TagId {
		get;
	}

	/// <summary>
	/// タグ分類
	/// </summary>
	public int TagCategoryId {
		get;
	}

	public TagCategoryModel TagCategory {
		get;
	}

	/// <summary>
	/// タグ名
	/// </summary>
	public string TagName {
		get;
	}

	/// <summary>
	/// タグ説明
	/// </summary>
	public string Detail {
		get;
	}

	public string Romaji {
		get;
	}

	public List<TagAliasModel> TagAliases {
		get;
	}

	public BindableReactiveProperty<string?> RepresentativeText {
		get;
	} = new();
}

public class TagCategoryModel {
	public TagCategoryModel(TagCategory tagCategory) {
		this.TagCategoryId = tagCategory.TagCategoryId;
		this.TagCategoryName = tagCategory.TagCategoryName;
		this.Detail = tagCategory.Detail;
	}

	/// <summary>
	/// タグ分類ID
	/// </summary>
	public int TagCategoryId {
		get;
	}

	/// <summary>
	/// タグ分類名
	/// </summary>
	public string TagCategoryName {
		get;
	}

	/// <summary>
	/// タグ分類の説明
	/// </summary>
	public string Detail {
		get;
	}
}

public class TagAliasModel {
	public TagAliasModel(TagAlias tagAlias) {
		this.TagAliasId = tagAlias.TagAliasId;
		this.TagId = tagAlias.TagId;
		this.Alias = tagAlias.Alias;
		this.Ruby = tagAlias.Ruby;
		this.Romaji = (tagAlias.Ruby ?? tagAlias.Alias.KatakanaToHiragana()).HiraganaToRomaji();
	}
	/// <summary>
	/// タグ別名ID
	/// </summary>
	public int TagAliasId {
		get;
	}

	/// <summary>
	/// タグID
	/// </summary>
	public int TagId {
		get;
	}

	/// <summary>
	/// 別名
	/// </summary>
	public string Alias {
		get;
	}

	/// <summary>
	/// 読み仮名
	/// </summary>
	public string? Ruby {
		get;
	}

	public string? Romaji {
		get;
	}
}
