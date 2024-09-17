using Microsoft.UI.Xaml.Data;

using PixChest.Utils.Enums;
using System.Collections.Generic;

namespace PixChest.Views.Resources.Converters; 

public class SortKeyToDisplayNameConverter : IValueConverter {
	private static readonly Dictionary<SortItemKeys, string> _names = new() {
		{SortItemKeys.FileName,"File Name" },
		{SortItemKeys.FilePath,"File Path" },
		{SortItemKeys.CreationTime,"Creation Time" },
		{SortItemKeys.ModifiedTime,"Modified Time" },
		{SortItemKeys.LastAccessTime,"Last Access Time" },
		{SortItemKeys.FileSize,"File Size" },
		{SortItemKeys.Location,"Location" },
		{SortItemKeys.Rate,"Rate" },
		{SortItemKeys.Resolution,"Resolution" },
	};

	public object? Convert(object value, Type targetType, object parameter, string language) {
		if (value is SortItemKeys sik) {
			return _names[sik];
		}

		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}

}
