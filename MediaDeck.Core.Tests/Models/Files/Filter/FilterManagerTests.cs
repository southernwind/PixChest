using MediaDeck.Composition.Stores.Config.Model;
using MediaDeck.Composition.Stores.State.Model.Objects;
using MediaDeck.Core.Models.Files.Filter;
using MediaDeck.Core.Stores.Config;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MediaDeck.Core.Tests.Models.Files.Filter;

/// <summary>
/// <see cref="FilterManager"/> のユニットテスト
/// </summary>
public class FilterManagerTests {
	private readonly Mock<IConfigStore> _mockConfigStore;
	private readonly SearchDefinitionsConfigModel _searchDefinitions;

	public FilterManagerTests() {
		var services = new ServiceCollection();
		services.AddTransient<FilterObject>();
		services.AddTransient<SortObject>(sp => new SortObject(sp));
		services.AddTransient<SortItemObject>();
		services.AddSingleton<MediaDeck.Composition.Interfaces.IStringProvider, StubStringProvider>();

		var serviceProvider = services.BuildServiceProvider();

		var stringProvider = serviceProvider.GetRequiredService<MediaDeck.Composition.Interfaces.IStringProvider>();
		this._searchDefinitions = new SearchDefinitionsConfigModel(serviceProvider, stringProvider);

		this._mockConfigStore = new Mock<IConfigStore>();
	}

	/// <summary>
	/// コンストラクタが <see cref="SearchDefinitionsConfigModel"/> からフィルタリング条件を正しく取得することを確認します。
	/// </summary>
	[Fact]
	public void Constructor_SetsFilteringConditionsFromSearchDefinitions() {
		// Arrange
		var filterObj = new FilterObject();
		this._searchDefinitions.FilteringConditions.Add(filterObj);

		// Act
		var filterManager = new FilterManager(this._mockConfigStore.Object, this._searchDefinitions);

		// Assert
		Assert.Single(filterManager.FilteringConditions);
		Assert.Equal(filterObj, filterManager.FilteringConditions[0].FilterObject);
	}

	/// <summary>
	/// <see cref="FilterManager.AddCondition"/> が新しいフィルタリング条件を追加することを確認します。
	/// </summary>
	[Fact]
	public void AddCondition_AddsNewFilteringCondition() {
		// Arrange
		var filterManager = new FilterManager(this._mockConfigStore.Object, this._searchDefinitions);
		var initialCount = filterManager.FilteringConditions.Count;
		var initialDefCount = this._searchDefinitions.FilteringConditions.Count;

		// Act
		filterManager.AddCondition();

		// Assert
		Assert.Equal(initialCount + 1, filterManager.FilteringConditions.Count);
		Assert.Equal(initialDefCount + 1, this._searchDefinitions.FilteringConditions.Count);
	}

	/// <summary>
	/// <see cref="FilterManager.RemoveCondition"/> が指定したフィルタリング条件を削除することを確認します。
	/// </summary>
	[Fact]
	public void RemoveCondition_RemovesTargetFilteringCondition() {
		// Arrange
		var filterManager = new FilterManager(this._mockConfigStore.Object, this._searchDefinitions);
		filterManager.AddCondition();
		var conditionToRemove = filterManager.FilteringConditions.Last();
		var initialCount = filterManager.FilteringConditions.Count;
		var initialDefCount = this._searchDefinitions.FilteringConditions.Count;

		// Act
		filterManager.RemoveCondition(conditionToRemove);

		// Assert
		Assert.Equal(initialCount - 1, filterManager.FilteringConditions.Count);
		Assert.Equal(initialDefCount - 1, this._searchDefinitions.FilteringConditions.Count);
		Assert.DoesNotContain(conditionToRemove, filterManager.FilteringConditions);
	}

	/// <summary>
	/// <see cref="FilterManager.Save"/> メソッドが <see cref="IConfigStore.Save"/> を呼び出すことを確認します。
	/// </summary>
	[Fact]
	public void Save_CallsConfigStoreSave() {
		// Arrange
		var filterManager = new FilterManager(this._mockConfigStore.Object, this._searchDefinitions);

		// Act
		filterManager.Save();

		// Assert
		this._mockConfigStore.Verify(x => x.Save(), Times.Once);
	}
}