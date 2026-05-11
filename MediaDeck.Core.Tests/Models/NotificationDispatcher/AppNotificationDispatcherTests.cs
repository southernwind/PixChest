using MediaDeck.Core.Models.NotificationDispatcher;
using R3;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.NotificationDispatcher;

/// <summary>
/// <see cref="AppNotificationDispatcher"/> のテストクラス
/// </summary>
public class AppNotificationDispatcherTests {
	/// <summary>
	/// 通知が正しく発行され、購読者が受信できることを検証します。
	/// </summary>
	[Fact]
	public void Notify_EmitsNotificationToSubscriber() {
		// Arrange
		var dispatcher = new AppNotificationDispatcher();
		AppNotification? receivedNotification = null;
		using var disposable = dispatcher.Notify.Subscribe(n => receivedNotification = n);
		var notification = AppNotification.Info("Test Message", "Test Title");

		// Act
		dispatcher.Notify.OnNext(notification);

		// Assert
		receivedNotification.ShouldNotBeNull();
		receivedNotification.Message.ShouldBe("Test Message");
		receivedNotification.Title.ShouldBe("Test Title");
		receivedNotification.Severity.ShouldBe(NotificationSeverity.Informational);
	}

	/// <summary>
	/// 複数の購読者が同じ通知を受信できることを検証します。
	/// </summary>
	[Fact]
	public void Notify_EmitsNotificationToMultipleSubscribers() {
		// Arrange
		var dispatcher = new AppNotificationDispatcher();
		var receivedCount = 0;
		using var disposable1 = dispatcher.Notify.Subscribe(_ => receivedCount++);
		using var disposable2 = dispatcher.Notify.Subscribe(_ => receivedCount++);
		var notification = AppNotification.Success("Success Message");

		// Act
		dispatcher.Notify.OnNext(notification);

		// Assert
		receivedCount.ShouldBe(2);
	}

	/// <summary>
	/// 購読解除後は通知を受信しないことを検証します。
	/// </summary>
	[Fact]
	public void Notify_DoesNotEmitToUnsubscribedObserver() {
		// Arrange
		var dispatcher = new AppNotificationDispatcher();
		var receivedCount = 0;
		var disposable = dispatcher.Notify.Subscribe(_ => receivedCount++);
		var notification = AppNotification.Warning("Warning Message");

		// Act
		dispatcher.Notify.OnNext(notification);
		disposable.Dispose();
		dispatcher.Notify.OnNext(notification);

		// Assert
		receivedCount.ShouldBe(1);
	}
}
