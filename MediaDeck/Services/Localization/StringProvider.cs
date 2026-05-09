using MediaDeck.ViewModels.Localization;

using Microsoft.Windows.ApplicationModel.Resources;

namespace MediaDeck.Services.Localization;

/// <summary>
/// <see cref="ResourceLoader"/> をラップする <see cref="IStringProvider"/> 実装。
/// Strings/{lang}/Resources.resw からの値解決を一元化する。
/// </summary>
[Inject(InjectServiceLifetime.Singleton, typeof(IStringProvider))]
public class StringProvider : IStringProvider {
	private readonly ResourceLoader _resourceLoader;

	public StringProvider() {
		// View 非依存の既定リソースマップ（"Resources"）を利用する。
		// PrimaryLanguageOverride は App 起動時に設定される前提。
		this._resourceLoader = new ResourceLoader();
	}

	public string GetString(string key) {
		if (string.IsNullOrEmpty(key)) {
			return string.Empty;
		}
		var value = this._resourceLoader.GetString(key);
		return string.IsNullOrEmpty(value) ? key : value;
	}

	public string GetString(string key, params object?[] args) {
		var format = this.GetString(key);
		if (args == null || args.Length == 0) {
			return format;
		}
		try {
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args);
		} catch (FormatException) {
			return format;
		}
	}
}