using System.Linq.Expressions;

using PixChest.Database.Tables;

namespace PixChest.Models.Repositories.Objects; 
public class FolderRepositoryConditionObject : RepositoryConditionObject {
	public required string DirectoryPath {
		get;
		init;
	}

	public override Expression<Func<MediaFile, bool>> WherePredicate() {
		if (this.DirectoryPath == null) {
			throw new InvalidOperationException();
		}
		return mediaFile => mediaFile.DirectoryPath.StartsWith(this.DirectoryPath);
	}
}
