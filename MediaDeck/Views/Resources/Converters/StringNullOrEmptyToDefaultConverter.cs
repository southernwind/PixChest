using CommunityToolkit.Mvvm.DependencyInjection;

using MediaDeck.Composition.Interfaces;

using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// 文字列が null または空の場合に、指定したデフォルトの多言語リソースを返すコンバータ。
/// </summary>
public class StringNullOrEmptyToDefaultConverter : IValueConverter {
	public object? Convert(object value, Type targetType, object parameter, string language) {
		var str = value as string;
		if (string.IsNullOrEmpty(str)) {
			var stringProvider = Ioc.Default.GetRequiredService<IStringProvider>();
			var key = parameter as string ?? "Common_Untitled";
			return stringProvider.GetString(key);
		}
		return str;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}