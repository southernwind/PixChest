using CommunityToolkit.Mvvm.DependencyInjection;

using MediaDeck.Composition.Interfaces;

using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// 文字列が null または空の場合に、指定したデフォルトの多言語リソースを返すコンバータ。
/// </summary>
public class StringNullOrEmptyToDefaultConverter : IValueConverter {
	private IStringProvider? _stringProvider;

	/// <summary>
	/// 多言語リソースプロバイダーのキャッシュ。初回アクセス時に解決される。
	/// </summary>
	private IStringProvider StringProvider {
		get {
			return this._stringProvider ??= Ioc.Default.GetRequiredService<IStringProvider>();
		}
	}

	public object? Convert(object value, Type targetType, object parameter, string language) {
		var str = value as string;
		if (string.IsNullOrEmpty(str)) {
			var key = parameter as string ?? "Common_Untitled";
			return this.StringProvider.GetString(key);
		}
		return str;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}