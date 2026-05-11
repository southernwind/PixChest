using System.Drawing;
using Shouldly;
using Rectangle = MediaDeck.Core.Models.Maps.Rectangle;

namespace MediaDeck.Core.Tests.Models.Maps;

/// <summary>
/// <see cref="Rectangle"/> のテストクラス
/// </summary>
public class RectangleTests {
	/// <summary>
	/// 重なっている矩形同士の距離が0になることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_OverlappingRectangles_ReturnsZero() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(50, 50), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(0);
	}

	/// <summary>
	/// 隣接している矩形同士の距離が0になることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_AdjacentRectangles_ReturnsZero() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(100, 0), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(0);
	}

	/// <summary>
	/// 包含関係にある矩形同士の距離が0になることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_RectanglesInsideEachOther_ReturnsZero() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(25, 25), new Size(50, 50));

		var distance1 = rect1.DistanceTo(rect2);
		var distance2 = rect2.DistanceTo(rect1);

		distance1.ShouldBe(0);
		distance2.ShouldBe(0);
	}

	/// <summary>
	/// 水平方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesHorizontally_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(200, 0), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(100);
	}

	/// <summary>
	/// 垂直方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesVertically_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(0, 200), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(100);
	}

	/// <summary>
	/// 右下方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesDiagonally_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(200, 200), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(Math.Sqrt(20000), 0.0001);
	}

	/// <summary>
	/// 左方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesLeft_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(200, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(0, 0), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(100);
	}

	/// <summary>
	/// 上方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesTop_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(0, 200), new Size(100, 100));
		var rect2 = new Rectangle(new Point(0, 0), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(100);
	}

	/// <summary>
	/// 同一の矩形同士の距離が0になることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_IdenticalRectangles_ReturnsZero() {
		var rect = new Rectangle(new Point(10, 10), new Size(50, 50));

		var distance = rect.DistanceTo(rect);

		distance.ShouldBe(0);
	}

	/// <summary>
	/// サイズ0の矩形（点）との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_ZeroSizeRectangle_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(0, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(200, 200), new Size(0, 0));

		var distance = rect1.DistanceTo(rect2);

		// (200,200) と (100,100) の距離 = sqrt(100^2 + 100^2) = sqrt(20000)
		distance.ShouldBe(Math.Sqrt(20000), 0.0001);
	}

	/// <summary>
	/// 負の座標を持つ矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_NegativeCoordinates_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(-100, -100), new Size(50, 50));
		var rect2 = new Rectangle(new Point(0, 0), new Size(50, 50));

		var distance = rect1.DistanceTo(rect2);

		// rect1: x=[-100, -50], y=[-100, -50]
		// rect2: x=[0, 50], y=[0, 50]
		// 距離は (-50, -50) と (0, 0) の距離 = sqrt(50^2 + 50^2) = sqrt(5000)
		distance.ShouldBe(Math.Sqrt(5000), 0.0001);
	}

	/// <summary>
	/// 左上方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesTopLeft_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(200, 200), new Size(100, 100));
		var rect2 = new Rectangle(new Point(0, 0), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(Math.Sqrt(20000), 0.0001);
	}

	/// <summary>
	/// 右上方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesTopRight_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(0, 200), new Size(100, 100));
		var rect2 = new Rectangle(new Point(200, 0), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(Math.Sqrt(20000), 0.0001);
	}

	/// <summary>
	/// 左下方向に離れた矩形との距離が正しく計算されることを検証します。
	/// </summary>
	[Fact]
	public void DistanceTo_DistantRectanglesBottomLeft_ReturnsCorrectDistance() {
		var rect1 = new Rectangle(new Point(200, 0), new Size(100, 100));
		var rect2 = new Rectangle(new Point(0, 200), new Size(100, 100));

		var distance = rect1.DistanceTo(rect2);

		distance.ShouldBe(Math.Sqrt(20000), 0.0001);
	}
}
