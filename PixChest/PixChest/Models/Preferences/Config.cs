using System.Collections.Generic;
using System.IO;
using System.Xaml;

using PixChest.Models.Preferences.CustomConfigs;

namespace PixChest.Models.Preferences;

[AddSingleton]
public class Config {
	private string? _configFilePath;
	/// <summary>
	/// 一般設定
	/// </summary>
	public PathConfig PathConfig {
		get;
		set;
	}

	/// <summary>
	/// スキャン設定
	/// </summary>
	public ScanConfig ScanConfig {
		get;
		set;
	}

	[Obsolete("for serialize")]
	public Config() {
		this.ScanConfig = null!;
		this.PathConfig = null!;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public Config(PathConfig pathConfig,ScanConfig scanConfig) {
		this.ScanConfig = scanConfig;
		this.PathConfig = pathConfig;
	}

	/// <summary>
	/// ファイルパス設定
	/// </summary>
	/// <param name="path">パス</param>
	public void SetFilePath(string path) {
		this._configFilePath = path;
	}

	/// <summary>
	/// 保存
	/// </summary>
	public void Save() {
		if (this._configFilePath is null) {
			throw new InvalidOperationException();
		}
		using var ms = new MemoryStream();
		var d = new SettingsBase[] {
			this.ScanConfig,
			this.PathConfig
		}.ToDictionary(x => x.GetType(), x => x.Export());
		XamlServices.Save(ms, d);
		using var fs = File.Create(this._configFilePath);
		ms.WriteTo(fs);
	}

	/// <summary>
	/// 設定ロード
	/// </summary>
	public void Load() {
		if (this._configFilePath is null) {
			throw new InvalidOperationException();
		}
		this.LoadDefault();
		if (!File.Exists(this._configFilePath)) {
			return;
		}

		if (XamlServices.Load(this._configFilePath) is not Dictionary<Type, Dictionary<string, dynamic>> config) {
			return;
		}

		foreach (var s in new SettingsBase[] { this.ScanConfig }) {
			if (config.TryGetValue(s.GetType(), out var d)) {
				s.Import(d);
			}
		}
	}

	/// <summary>
	/// デフォルト設定ロード
	/// </summary>
	private void LoadDefault() {
		this.PathConfig.LoadDefault();
		this.ScanConfig.LoadDefault();
	}

	/// <summary>
	/// 破棄
	/// </summary>
	public void Dispose() {
		this.PathConfig.Dispose();
		this.ScanConfig.Dispose();
	}
}
