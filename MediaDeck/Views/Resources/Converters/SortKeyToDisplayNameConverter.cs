using CommunityToolkit.Mvvm.DependencyInjection;

using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces;

using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// ソートキーを多言語対応された表示名に変換するコンバータ。
/// </summary>
public class SortKeyToDisplayNameConverter : IValueConverter {
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
		if (value is SortItemKey sik) {
			return this.StringProvider.GetString($"SortItemKey_{sik}");
		}

		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}