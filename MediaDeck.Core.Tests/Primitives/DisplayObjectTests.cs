using MediaDeck.Core.Primitives;
using Shouldly;

namespace MediaDeck.Core.Tests.Primitives;

/// <summary>
///     <see cref="DisplayObject{T}"/> のテストクラスです。
/// </summary>
public class DisplayObjectTests {
	/// <summary>
	///     コンストラクタで表示文字列と値が正しく設定されることを確認します。
	/// </summary>
	[Fact]
	public void Constructor_SetsDisplayStringAndValue() {
		// Arrange & Act
		var obj = new DisplayObject<int>("表示テキスト", 42);

		// Assert
		obj.DisplayString.ShouldBe("表示テキスト");
		obj.Value.ShouldBe(42);
	}

	/// <summary>
	///     文字列型の値でも正しく動作することを確認します。
	/// </summary>
	[Fact]
	public void Constructor_StringValue_WorksCorrectly() {
		// Arrange & Act
		var obj = new DisplayObject<string>("ラベル", "データ");

		// Assert
		obj.DisplayString.ShouldBe("ラベル");
		obj.Value.ShouldBe("データ");
	}

	/// <summary>
	///     null値でも正しく動作することを確認します。
	/// </summary>
	[Fact]
	public void Constructor_NullValue_WorksCorrectly() {
		// Arrange & Act
		var obj = new DisplayObject<string?>("空", null);

		// Assert
		obj.DisplayString.ShouldBe("空");
		obj.Value.ShouldBeNull();
	}
}