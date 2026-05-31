using MediaDeck.Composition.Objects;
using MediaDeck.Core.Models.NotificationDispatcher;
using R3;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.NotificationDispatcher;

public class AppNotificationServiceTests {
	/// <summary>
	///     Notifyがディスパッチャーに通知を転送し、TargetWindowIdが設定されることを確認します。
	/// </summary>
	[Fact]
	public void Notify_SetsTargetWindowIdAndDispatchesToDispatcher() {
		// Arrange
		var dispatcher = new AppNotificationDispatcher();
		var windowId = Guid.NewGuid();
		var contextProvider = new NotificationContextProvider {
			TargetWindowIdResolver = () => windowId
		};
		var service = new AppNotificationService(dispatcher, contextProvider);

		AppNotification? received = null;
		using var sub = dispatcher.Notify.Subscribe(x => received = x);

		var notification = new AppNotification { Message = "test message" };

		// Act
		service.Notify(notification);

		// Assert
		received.ShouldNotBeNull();
		received.Message.ShouldBe("test message");
		received.TargetWindowId.ShouldBe(windowId);
	}

	/// <summary>
	///     TargetWindowIdResolverがnullを返す場合、TargetWindowIdがnullのまま配信されることを確認します。
	/// </summary>
	[Fact]
	public void Notify_WhenResolverReturnsNull_TargetWindowIdIsNull() {
		// Arrange
		var dispatcher = new AppNotificationDispatcher();
		var contextProvider = new NotificationContextProvider {
			TargetWindowIdResolver = () => null
		};
		var service = new AppNotificationService(dispatcher, contextProvider);

		AppNotification? received = null;
		using var sub = dispatcher.Notify.Subscribe(x => received = x);

		// Act
		service.Notify(new AppNotification { Message = "broadcast" });

		// Assert
		received.ShouldNotBeNull();
		received.TargetWindowId.ShouldBeNull();
	}
}