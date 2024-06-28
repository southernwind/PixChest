using System.Collections.Generic;
using System.IO;
using System.Xaml;

using PixChest.Models.Preferences.CustomStates;

namespace PixChest.Models.Settings;
/// <summary>
/// 状態
/// </summary>
[AddSingleton]
public class States {
	private string? _statesFilePath;


	/// <summary>
	/// 検索の状態
	/// </summary>
	public SearchStates SearchStates {
		get;
		set;
	}

	[Obsolete("for serialize")]
	public States() {
		this.SearchStates = null!;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public States(SearchStates searchStates) {
		this.SearchStates = searchStates;
	}

	/// <summary>
	/// ファイルパス設定
	/// </summary>
	/// <param name="path">パス</param>
	public void SetFilePath(string path) {
		this._statesFilePath = path;
	}

	/// <summary>
	/// 保存
	/// </summary>
	public void Save() {
		if (this._statesFilePath == null) {
			throw new InvalidOperationException();
		}
		using var ms = new MemoryStream();
		var d = new SettingsBase[] {
			this.SearchStates
		}.ToDictionary(x => x.GetType(), x => x.Export());
		XamlServices.Save(ms, d);
		using var fs = File.Create(this._statesFilePath);
		ms.WriteTo(fs);
	}

	/// <summary>
	/// ロード
	/// </summary>
	public void Load() {
		this.LoadDefault();
		if (!File.Exists(this._statesFilePath)) {
			return;
		}

		if (XamlServices.Load(this._statesFilePath) is not Dictionary<Type, Dictionary<string, dynamic>> states) {
			return;
		}
		foreach (var s in new SettingsBase[] { this.SearchStates }) {
			s.Import(states[s.GetType()]);
		}
	}

	/// <summary>
	/// デフォルトロード
	/// </summary>
	private void LoadDefault() {
		this.SearchStates.LoadDefault();
	}
}
