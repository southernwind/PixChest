using MediaDeck.Composition.Objects;
using MediaDeck.Core.Models.NotificationDispatcher;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.NotificationDispatcher;

/// <summary>
///     <see cref="InfoBarNotification"/> のテストクラスです。
/// </summary>
public class InfoBarNotificationTests {
	/// <summary>
	///     Infoファクトリメソッドが情報レベルの通知を生成することを確認します。
	/// </summary>
	[Fact]
	public void Info_CreatesInformationalNotification() {
		// Act
		var notification = InfoBarNotification.Info("テストメッセージ", "タイトル");

		// Assert
		notification.Message.ShouldBe("テストメッセージ");
		notification.Title.ShouldBe("タイトル");
		notification.Severity.ShouldBe(NotificationSeverity.Informational);
		notification.AutoCloseMilliseconds.ShouldBe(3000);
	}

	/// <summary>
	///     Successファクトリメソッドが成功レベルの通知を生成することを確認します。
	/// </summary>
	[Fact]
	public void Success_CreatesSuccessNotification() {
		// Act
		var notification = InfoBarNotification.Success("成功", "OK");

		// Assert
		notification.Message.ShouldBe("成功");
		notification.Severity.ShouldBe(NotificationSeverity.Success);
		notification.AutoCloseMilliseconds.ShouldBe(3000);
	}

	/// <summary>
	///     Warningファクトリメソッドが警告レベルの通知を生成することを確認します。
	/// </summary>
	[Fact]
	public void Warning_CreatesWarningNotification() {
		// Act
		var notification = InfoBarNotification.Warning("警告メッセージ");

		// Assert
		notification.Message.ShouldBe("警告メッセージ");
		notification.Severity.ShouldBe(NotificationSeverity.Warning);
		notification.AutoCloseMilliseconds.ShouldBe(5000);
	}

	/// <summary>
	///     Errorファクトリメソッドがエラーレベルの通知を生成することを確認します。
	/// </summary>
	[Fact]
	public void Error_CreatesErrorNotification() {
		// Act
		var notification = InfoBarNotification.Error("エラー発生");

		// Assert
		notification.Message.ShouldBe("エラー発生");
		notification.Severity.ShouldBe(NotificationSeverity.Error);
		notification.AutoCloseMilliseconds.ShouldBe(0);
	}

	/// <summary>
	///     AutoCloseMillisecondsをカスタム値で指定できることを確認します。
	/// </summary>
	[Fact]
	public void Info_CustomAutoClose_SetsCorrectly() {
		// Act
		var notification = InfoBarNotification.Info("msg", autoCloseMs: 10000);

		// Assert
		notification.AutoCloseMilliseconds.ShouldBe(10000);
	}

	/// <summary>
	///     FromAppNotificationが正しく変換することを確認します。
	/// </summary>
	[Fact]
	public void FromAppNotification_ConvertsCorrectly() {
		// Arrange
		var appNotif = new AppNotification {
			Message = "メッセージ",
			Title = "タイトル",
			Severity = NotificationSeverity.Warning,
			AutoCloseMilliseconds = 7000
		};

		// Act
		var result = InfoBarNotification.FromAppNotification(appNotif);

		// Assert
		result.Message.ShouldBe("メッセージ");
		result.Title.ShouldBe("タイトル");
		result.Severity.ShouldBe(NotificationSeverity.Warning);
		result.AutoCloseMilliseconds.ShouldBe(7000);
	}
}