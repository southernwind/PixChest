using CommunityToolkit.Mvvm.DependencyInjection;

using MediaDeck.Composition.Interfaces;
using MediaDeck.Core.Services.FileChangeMonitor;

using Microsoft.UI.Xaml.Data;

namespace MediaDeck.Views.Resources.Converters;

/// <summary>
/// FileChangeTypeを多言語対応された表示名に変換するコンバーター。
/// </summary>
public class FileChangeTypeToDisplayNameConverter : IValueConverter {
	private IStringProvider? _stringProvider;

	private IStringProvider StringProvider {
		get {
			return this._stringProvider ??= Ioc.Default.GetRequiredService<IStringProvider>();
		}
	}

	/// <summary>
	/// FileChangeType列挙値を取得し、対応する表示名に変換します。
	/// </summary>
	/// <param name="value">FileChangeType列挙値</param>
	/// <param name="targetType">変換先型（string）</param>
	/// <param name="parameter">変換パラメータ（不使用）</param>
	/// <param name="language">言語（不使用）</param>
	/// <returns>表示名文字列</returns>
	public object Convert(object? value, Type targetType, object parameter, string language) {
		if (value is FileChangeType changeType) {
			return this.StringProvider.GetString($"FileChangeType_{changeType}");
		}
		return this.StringProvider.GetString("FileChangeType_Unknown");
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language) {
		throw new NotImplementedException();
	}
}