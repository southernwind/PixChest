using System.Linq.Expressions;

using MediaDeck.Composition.Interfaces.Files;
using MediaDeck.Composition.Interfaces.Notifications;
using MediaDeck.Composition.Tables;

using R3.JsonConfig.Attributes;

namespace MediaDeck.Core.Models.Files.SearchConditions;

[GenerateR3JsonConfigDto]
[JsonConfigDerivedType("folder")]
[Inject(InjectServiceLifetime.Transient)]
[Inject(InjectServiceLifetime.Transient, typeof(IFolderSearchCondition))]
[Inject(InjectServiceLifetime.Transient, typeof(IRepositorySearchCondition))]
public class FolderSearchCondition : ISearchCondition, IFolderSearchCondition, IRepositorySearchCondition {
	public string FolderPath {
		get {
			return field ?? throw new InvalidOperationException($"{nameof(this.FolderPath)} is not initialized.");
		}
		set {
			field = value;
		}
	}

	public bool IncludeSubDirectories {
		get;
		set;
	}

	public string DisplayText {
		get {
			return $"Folder={this.FolderPath}{(this.IncludeSubDirectories ? "&IncludeSubFolders" : "")}";
		}
	}

	public Expression<Func<MediaItem, bool>>? WherePredicate {
		get {
			if (this.IncludeSubDirectories) {
				return MediaItem =>
					MediaItem.DirectoryPath == this.FolderPath || MediaItem.DirectoryPath.StartsWith($"{this.FolderPath}{System.IO.Path.DirectorySeparatorChar}");
			} else {
				return MediaItem =>
					MediaItem.DirectoryPath == this.FolderPath;
			}
		}
	}

	public bool IsMatchForSuggest(string searchWord) {
		return this.FolderPath.Contains(searchWord, StringComparison.CurrentCultureIgnoreCase);
	}
}