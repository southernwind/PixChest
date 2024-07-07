using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;

namespace PixChest.Models.Preferences.CustomStates;
/// <summary>
/// 検索状態
/// </summary>

[AddSingleton]
public class SearchStates : SettingsBase {
	/// <summary>
	/// カレントフィルター条件
	/// </summary>
	public SettingsItem<FilterObject?> CurrentFilteringCondition {
		get;
	} = new SettingsItem<FilterObject?>(null);

	/// <summary>
	/// フィルター条件リスト
	/// </summary>
	public SettingsCollection<FilterObject> FilteringConditions {
		get;
	} = new SettingsCollection<FilterObject>([]) { MaybeEditMember = true };
}