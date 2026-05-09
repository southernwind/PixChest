namespace MediaDeck.ViewModels.Localization;

/// <summary>
/// ローカライズ済み文字列リソースへの薄いアクセサ。
/// ViewModel / Service など UI 非依存層から
/// Resources.resw のキーを安全に解決するために利用する。
/// </summary>
public interface IStringProvider {
	/// <summary>
	/// 指定したリソースキーに対応するローカライズ文字列を取得する。
	/// 見つからない場合は <paramref name="key"/> をそのまま返す。
	/// </summary>
	/// <param name="key">resw のキー（例: "ConfigWindow_Title"）。</param>
	public string GetString(string key);

	/// <summary>
	/// <see cref="string.Format(System.IFormatProvider, string, object?[])"/> 互換のフォーマット適用版。
	/// </summary>
	public string GetString(string key, params object?[] args);
}