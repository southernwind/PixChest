using System.Linq.Expressions;

using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Interfaces.Notifications;
using MediaDeck.Composition.Tables;

using R3.JsonConfig.Attributes;

namespace MediaDeck.Core.Models.Files.SearchConditions;

[GenerateR3JsonConfigDto]
[JsonConfigDerivedType("album")]
[Inject(InjectServiceLifetime.Transient)]
[Inject(InjectServiceLifetime.Transient, typeof(IRepositorySearchCondition))]
public class AlbumSearchCondition : ISearchCondition, IRepositorySearchCondition {
	public string AlbumPath {
		get {
			return field ?? throw new InvalidOperationException($"{nameof(this.AlbumPath)} is not initialized.");
		}
		set {
			field = value;
		}
	}

	public bool IncludeSubAlbums {
		get;
		set;
	}

	public string DisplayText {
		get {
			return $"Album={this.AlbumPath}{(this.IncludeSubAlbums ? "&IncludeSubAlbums" : "")}";
		}
	}

	public Expression<Func<MediaItem, bool>>? WherePredicate {
		get {
			if (this.IncludeSubAlbums) {
				var prefix = $"{this.AlbumPath}/";
				return MediaItem =>
					MediaItem.MediaItemAlbums.Any(mia =>
						mia.Album.Path == this.AlbumPath || mia.Album.Path.StartsWith(prefix));
			} else {
				return MediaItem =>
					MediaItem.MediaItemAlbums.Any(mia => mia.Album.Path == this.AlbumPath);
			}
		}
	}

	public bool IsMatchForSuggest(string searchWord) {
		return this.AlbumPath.Contains(searchWord, StringComparison.CurrentCultureIgnoreCase);
	}
}