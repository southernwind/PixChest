using System.Collections.Generic;
using System.IO;

using PixChest.Utils.Objects;

namespace PixChest.Models.Repositories.Objects;

public partial class FolderObject {
	public string FolderPath {
		get;
	}

	public FolderObject? Parent {
		get;
	}

	public FolderObject[] ChildFolders {
		get;
	}

	public string FolderName {
		get;
	}

	public long FileCount {
		get;
	}

	public bool IsExpanded {
		get;
	} = false;

	public FolderObject(FolderObject? parent, string currentPath, ValueCountPair<string>[] directories) {
		this.Parent = parent;
		this.FolderPath = currentPath;

		// 子フォルダを設定
		var children = new List<FolderObject>();
		foreach (var dir in directories) {
			if (dir.Value == this.FolderPath) {
				// 自分自身は除外
				continue;
			}
			if (children.Any(x => $"{dir.Value}{Path.DirectorySeparatorChar}".StartsWith(x.FolderPath))) {
				// すでに親が含まれている
				continue;
			}
			children.Add(new(this, dir.Value, directories.Where(x => x.Value.StartsWith(dir.Value)).ToArray()));
		}
		this.ChildFolders = [.. children];

		if (parent == null) {
			this.FolderName = "PC";
		} else {
			this.FolderName = this.FolderPath.Replace($"{parent.FolderPath}{Path.DirectorySeparatorChar}", "");
		}
		this.FileCount = directories.Sum(x => x.Count);
	}
}
