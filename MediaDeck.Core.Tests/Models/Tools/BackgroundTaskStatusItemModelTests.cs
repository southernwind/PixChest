using MediaDeck.Core.Models.Tools;
using R3;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Tools;

/// <summary>
///     <see cref="BackgroundTaskStatusItemModel"/> のテストクラスです。
/// </summary>
public class BackgroundTaskStatusItemModelTests {
	/// <summary>
	///     コンストラクタで全プロパティが正しく設定されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_SetsAllProperties() {
		// Arrange
		var completed = new ReactiveProperty<long>(5);
		var target = new ReactiveProperty<long>(10);
		var reRunCalled = false;
		var cancelCalled = false;

		// Act
		var item = new BackgroundTaskStatusItemModel(
			"テストタスク",
			completed,
			target,
			() => reRunCalled = true,
			() => cancelCalled = true);

		// Assert
		item.DisplayName.ShouldBe("テストタスク");
		item.CompletedCount.ShouldBe(completed);
		item.TargetCount.ShouldBe(target);
		item.CompletedCount.Value.ShouldBe(5);
		item.TargetCount.Value.ShouldBe(10);

		item.ReRun();
		reRunCalled.ShouldBeTrue();

		item.Cancel!();
		cancelCalled.ShouldBeTrue();
	}

	/// <summary>
	///     Cancelがnullの場合でも正しく動作することを確認します。
	/// </summary>
	[Fact]
	public void Constructor_NullCancel_IsAllowed() {
		// Arrange & Act
		var item = new BackgroundTaskStatusItemModel(
			"タスク",
			new ReactiveProperty<long>(0),
			new ReactiveProperty<long>(0),
			() => { });

		// Assert
		item.Cancel.ShouldBeNull();
	}
}