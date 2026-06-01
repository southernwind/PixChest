using System.ComponentModel;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Stores.State.Model.Objects;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Files.Sort;

/// <summary>
///     <see cref="SortItemObject"/> のテストクラスです。
/// </summary>
public class SortItemObjectTests {
	[Fact]
	public void Equals_SameKeyAndDirection_ReturnsTrue() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		var b = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		a.Equals(b).ShouldBeTrue();
	}

	[Fact]
	public void Equals_DifferentKey_ReturnsFalse() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		var b = new SortItemObject { SortItemKey = SortItemKey.FileSize, Direction = ListSortDirection.Ascending };
		a.Equals(b).ShouldBeFalse();
	}

	[Fact]
	public void Equals_DifferentDirection_ReturnsFalse() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		var b = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Descending };
		a.Equals(b).ShouldBeFalse();
	}

	[Fact]
	public void Equals_Null_ReturnsFalse() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		a.Equals((SortItemObject?)null).ShouldBeFalse();
	}

	[Fact]
	public void Equals_SameReference_ReturnsTrue() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		a.Equals(a).ShouldBeTrue();
	}

	[Fact]
	public void Equals_ObjectOverload_WorksCorrectly() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		var b = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		a.Equals((object)b).ShouldBeTrue();
	}

	[Fact]
	public void Equals_ObjectNull_ReturnsFalse() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		a.Equals((object?)null).ShouldBeFalse();
	}

	[Fact]
	public void GetHashCode_SameKeyAndDirection_ReturnsSameHash() {
		var a = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		var b = new SortItemObject { SortItemKey = SortItemKey.Rate, Direction = ListSortDirection.Ascending };
		a.GetHashCode().ShouldBe(b.GetHashCode());
	}
}