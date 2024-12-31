using System.ComponentModel;

using PixChest.Models.Files.Filter;
using PixChest.Models.Files.Sort;
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
		new SortObject("File Path", [new (SortItemKey.FilePath)]),
		new SortObject("Modified Time", [new (SortItemKey.ModifiedTime)]),
		new SortObject("Rate", [new(SortItemKey.Rate)]),
		new SortObject("Usage Count", [new(SortItemKey.UsageCount)]),
		new SortObject("File Size", [new(SortItemKey.FileSize)]));

	/// <summary>
	/// 全体ソート方向
	/// </summary>
	public SettingsItem<ListSortDirection> SortDirection {
		get;
	} = new(ListSortDirection.Ascending);
}