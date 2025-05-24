using System;
using System.Collections.Generic;
using System.Text;
using PixChest.Models.Files.SearchConditions;
using PixChest.Models.FolderManager;

namespace PixChest.Models.Preferences.CustomStates; 
/// <summary>
/// フォルダ管理状態
/// </summary>

[AddSingleton]
public class FolderManagerStates : SettingsBase {
	/// <summary>
	/// 管理対象フォルダリスト
	/// </summary>
	public SettingsCollection<FolderModel> Folders {
		get;
	} = new SettingsCollection<FolderModel>([]);
}
