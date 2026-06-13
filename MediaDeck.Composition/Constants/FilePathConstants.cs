namespace MediaDeck.Composition.Constants;

public static class FilePathConstants {
	private static readonly string DefaultBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MediaDeck");
	private static string? _baseDirectory;

	public static string BaseDirectory {
		get {
			return CurrentBaseDirectory;
		}
	}

	public static string NoThumbnailFilePath {
		get;
	} = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "thumbnail_creation_failed.png");

	public static string StateFilePath {
		get {
			return Path.Combine(CurrentBaseDirectory, "MediaDeck.states");
		}
	}

	public static string ConfigFilePath {
		get {
			return Path.Combine(CurrentBaseDirectory, "MediaDeck.config");
		}
	}

	public static string ThumbnailDirectoryPath {
		get {
			return Path.Combine(CurrentBaseDirectory, "thumbs");
		}
	}

	public static string DatabaseFilePath {
		get {
			return Path.Combine(CurrentBaseDirectory, "pix.db");
		}
	}

	public static void OverrideBaseDirectory(string? baseDirectory) {
		if (string.IsNullOrWhiteSpace(baseDirectory)) {
			throw new ArgumentException("Base directory must not be empty or whitespace.", nameof(baseDirectory));
		}

		_baseDirectory = baseDirectory;
	}

	private static string CurrentBaseDirectory {
		get {
			return _baseDirectory ?? DefaultBaseDirectory;
		}
	}
}