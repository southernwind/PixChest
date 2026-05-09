using MediaDeck.Composition.Interfaces;

namespace MediaDeck.Core.Tests;

public class StubStringProvider : IStringProvider {
	public string GetString(string key) {
		return key;
	}

	public string GetString(string key, params object?[] args) {
		return string.Format(key, args);
	}
}