using PixChest.Models.Files.Filter;
using PixChest.Models.Files.Sort;
using PixChest.Models.Repositories.Objects;
using PixChest.Utils.Enums;

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

	/// <summary>
	/// カレントリポジトリ条件
	/// </summary>
	public SettingsItem<RepositoryConditionObject?> CurrentRepositoryCondition {
		get;
	} = new (null);

	/// <summary>
	/// カレントソート条件
	/// </summary>
	public SettingsItem<SortObject?> CurrentSortCondition {
		get;
	} = new (null);

	/// <summary>
	/// ソート条件リスト
	/// </summary>
	public SettingsCollection<SortObject> SortConditions {
		get;
	} = new (
		new SortObject("File Path", [new (SortItemKeys.FilePath)]),
		new SortObject("Modified Time", [new (SortItemKeys.ModifiedTime)]),
		new SortObject("Rate", [new (SortItemKeys.Rate)]),
		new SortObject("File Size", [new (SortItemKeys.FileSize)]));

}