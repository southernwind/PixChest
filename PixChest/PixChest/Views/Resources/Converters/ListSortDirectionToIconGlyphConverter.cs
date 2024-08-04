using System.Collections.Generic;
using System.ComponentModel;

namespace PixChest.Views.Resources.Converters;

public class ListSortDirectionToIconGlyphConverter : DictionaryConverterBase<ListSortDirection, string> {
	public ListSortDirectionToIconGlyphConverter() {
		this.Dictionary = new Dictionary<ListSortDirection, string> {
			{ ListSortDirection.Ascending, "\xF0AD" },
			{ ListSortDirection.Descending, "\xF0AE" }
		};
	}
}
