using System.IO;
using System.Linq.Expressions;

using PixChest.Database.Tables;
using PixChest.FileTypes.Base.Models.Interfaces;
using PixChest.Models.Repositories.Objects;

namespace PixChest.Models.Files.SearchConditions;
public class FolderSearchCondition: ISearchCondition {
	public FolderSearchCondition(FolderObject folderObject) {
		this.FolderObject = folderObject;
	}

	public FolderObject FolderObject{
		get;
	}

	public bool IncludeSubDirectories {
		get;
		set;
	}

	public string DisplayText {
		get {
			return $"Folder={this.FolderObject.FolderPath}{(this.IncludeSubDirectories ? "&IncludeSubDirectories" : "")}";
		}
	}

	public Expression<Func<MediaFile, bool>>? WherePredicate {
		get {
			if (this.IncludeSubDirectories) {
				return mediaFile =>
					mediaFile.DirectoryPath == this.FolderObject.FolderPath || mediaFile.DirectoryPath.StartsWith($"{this.FolderObject.FolderPath}{Path.DirectorySeparatorChar}");
			} else {
				return mediaFile =>
					mediaFile.DirectoryPath == this.FolderObject.FolderPath;
			}
		}
	}
	public Func<IFileModel, bool>? Filter {
		get;
	} = null;
}
