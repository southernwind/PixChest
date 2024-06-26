using PixChest.Models.FilesFilter;
using PixChest.Models.Settings;

namespace PixChest.Models.Preferences.CustomStates;
/// <summary>
/// 検索状態
/// </summary>
public class SearchStates : SettingsBase {
	/// <summary>
	/// カレントフィルター条件
	/// </summary>
	public SettingsItemWithKey<string, FilterObject?> CurrentFilteringCondition {
		get;
	} = new SettingsItemWithKey<string, FilterObject?>([], null);

	/// <summary>
	/// フィルター条件リスト
	/// </summary>
	public SettingsCollection<FilterObject> FilteringConditions {
		get;
	} = new SettingsCollection<FilterObject>([]) { MaybeEditMember = true };
}