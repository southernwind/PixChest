using System.Collections.Generic;

using PixChest.Database.Tables;

namespace PixChest.Models.FileDetailManagers.Objects;
public class TagWithRomaji : Tag {
	public new required List<TagAliasWithRomaji> TagAliases {
		get;
		set;
	}
}
