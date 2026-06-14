using CommunityToolkit.Mvvm.DependencyInjection;

using MediaDeck.Composition.Interfaces;
using MediaDeck.ViewModels;

using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// FileChangeFilterを多言語対応された表示名に変換するコンバーター。
/// </summary>
public class FileChangeFilterToDisplayNameConverter : IValueConverter {
	private IStringProvider? _stringProvider;

	private IStringProvider StringProvider {
		get {
			return this._stringProvider ??= Ioc.Default.GetRequiredService<IStringProvider>();
		}
	}

	/// <summary>
	/// FileChangeFilterの列挙値を取得し、対応する表示名に変換します。
	/// </summary>
	/// <param name="value">FileChangeFilter列挙値</param>
	/// <param name="targetType">変換先型（string）</param>
	/// <param name="parameter">変換パラメータ（不使用）</param>
	/// <param name="language">言語（不使用）</param>
	/// <returns>表示名文字列</returns>
	public object Convert(object? value, Type targetType, object parameter, string language) {
		if (value is FileChangeFilter filter) {
			return this.StringProvider.GetString($"FileChangeFilter_{filter}");
		}
		return value?.ToString() ?? string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}