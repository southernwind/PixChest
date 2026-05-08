using MediaDeck.Common.Base;
using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Core.Stores.Config;

namespace MediaDeck.Core.Models.Files.Filter;

/// <summary>
/// フィルターマネージャー
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
[Inject(InjectServiceLifetime.Singleton)]
public class FilterManager : ModelBase {
	private readonly IConfigStore _configStore;
	private readonly SearchDefinitionsConfigModel _searchDefinitions;

	public FilterManager(IConfigStore configStore, SearchDefinitionsConfigModel searchDefinitions) {
		this._configStore = configStore;
		this._searchDefinitions = searchDefinitions;
		this.FilteringConditions = [.. searchDefinitions.FilteringConditions.Select(x => new FilteringConditionEditor(x))];
	}

	/// <summary>
	/// フィルター条件リスト
	/// </summary>
	public ObservableList<FilteringConditionEditor> FilteringConditions {
		get;
	}

	/// <summary>
	/// 保存
	/// </summary>
	public void Save() {
		this._configStore.Save();
	}

	/// <summary>
	/// フィルタリング条件追加
	/// </summary>
	public void AddCondition() {
		var fo = this._searchDefinitions.AddFilteringCondition();
		this.FilteringConditions.Add(new FilteringConditionEditor(fo));
	}

	/// <summary>
	/// フィルタリング条件削除
	/// </summary>
	/// <param name="filteringCondition">削除するフィルタリング条件</param>
	public void RemoveCondition(FilteringConditionEditor filteringCondition) {
		this._searchDefinitions.RemoveFilteringCondition(filteringCondition.FilterObject);
		this.FilteringConditions.Remove(filteringCondition);
	}
}