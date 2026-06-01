using MediaDeck.Core.Models.Maps;
using Shouldly;

namespace MediaDeck.Core.Tests.Models.Maps;

public class AddressTests {
	[Fact]
	public void Constructor_EmptyPositions_HasNoChildren() {
		var address = new Address([]);
		address.Children.ShouldBeEmpty();
		address.Count.ShouldBe(0);
	}

	[Fact]
	public void Properties_CanBeSetAndRead() {
		var address = new Address([]) {
			Name = "Tokyo",
			Type = "city",
			IsYet = false,
			IsFailure = false
		};
		address.Name.ShouldBe("Tokyo");
		address.Type.ShouldBe("city");
		address.IsYet.ShouldBeFalse();
		address.IsFailure.ShouldBeFalse();
	}

	[Fact]
	public void Parent_DefaultIsNull() {
		var address = new Address([]);
		address.Parent.ShouldBeNull();
	}

	[Fact]
	public void Parent_CanBeSet() {
		var parent = new Address([]) { Name = "Japan" };
		var child = new Address([]) { Parent = parent, Name = "Tokyo" };
		child.Parent.ShouldBe(parent);
	}
}