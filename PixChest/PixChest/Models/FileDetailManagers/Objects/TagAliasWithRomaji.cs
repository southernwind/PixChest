using PixChest.Database.Tables;

namespace PixChest.Models.FileDetailManagers.Objects;
public class TagAliasWithRomaji :TagAlias {
	public string? Romaji {
		get;
		set;
	}
}
