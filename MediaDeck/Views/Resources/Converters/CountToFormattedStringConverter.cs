using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// 数値を K, M などの単位付き文字列に変換するコンバーター
/// </summary>
public class CountToFormattedStringConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, string language) {
		if (value is not int count) {
			return value?.ToString() ?? "";
		}

		if (count >= 1000000) {
			return (count / 1000000.0).ToString("0.#") + "M";
		}
		if (count >= 1000) {
			return (count / 1000.0).ToString("0.#") + "K";
		}
		return count.ToString();
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}