using System.IO;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Models.Files;
using PixChest.Models.Files.SearchConditions;
using PixChest.Models.Repositories.Objects;
using PixChest.Utils.Notifications;
using PixChest.Utils.Objects;

namespace PixChest.Models.Repositories;

[AddTransient]
public class FolderRepository : RepositoryBase {
	public FolderRepository(PixChestDbContext dbContext, MediaContentLibrary mediaContentLibrary) {
		this._db = dbContext;
		this._mediaContentLibrary = mediaContentLibrary;
		FileNotifications
			.FileRegistered
			.ThrottleLast(TimeSpan.FromSeconds(10))
			.Subscribe(async _ => await this.Load());
	}

	private readonly PixChestDbContext _db;
	private readonly MediaContentLibrary _mediaContentLibrary;
	public ReactiveProperty<FolderObject> RootFolder {
		get;
	} = new();

	public string[] _currentDirectoryPathList = [];

	public override async Task Load() {
		var list = (await this._db
			.MediaFiles
			.GroupBy(x => x.DirectoryPath)
			.Select(x => new ValueCountPair<string>(x.Key, x.Count()))
			.ToListAsync())
			.OrderBy(x => x.Value)
			.ToList();

		var directoryList = list.Select(x => x.Value).ToArray();
		if (directoryList.SequenceEqual(this._currentDirectoryPathList)) {
			return;
		}
		this._currentDirectoryPathList = directoryList;

		var all = list.Select(x => (x.Value, x.Count, Split: x.Value.Split(Path.DirectorySeparatorChar))).ToArray();
		var maxPathDepth = all.Length == 0 ? 0 : all.Max(x => x.Split.Length);
		
		// ルート追加
		list.Add(new ("",list.Sum(x => x.Count)));
		// 足りない部分を補足
		for (var depth = 1; depth <= maxPathDepth; depth++) { // 最大の深さまで
			var target = all.Where(x => x.Split.Length > depth).ToArray(); // 深さが足りないものを除外
			list.AddRange(target
				.Select(x =>
					(
						item: x,
						joined: string.Join(Path.DirectorySeparatorChar, x.Split[0..depth]), // 対象の深さまでのパス
						joinedPlus: string.Join(Path.DirectorySeparatorChar, x.Split[0..(depth + 1)]) // 対象の深さ+1までのパス
					)
				)
				.Where(x => !all.Any(y => y.Value == x.joined)) // 対象のパスがすでに含まれていたら除外
				.GroupBy(x => x.joined) // 対象の深さまでのパスでグループ化
				.Where(x => x.DistinctBy(x => x.joinedPlus).Count() >= 2) // 対象の深さから、2つ以上のフォルダに分岐している
				.Select(x => new ValueCountPair<string>(x.Key, 0)));
		}

		this.RootFolder.Value = new FolderObject(null, "", [.. list.OrderBy(x => x.Value)]);
	}

	public void SetRepositoryCandidate(FolderObject folderObject,bool includeSubDirectory) {
		this._mediaContentLibrary.SearchConditions.RemoveRange(this._mediaContentLibrary.SearchConditions.Where(x => x is FolderSearchCondition));
		this._mediaContentLibrary.SearchConditions.Add(new FolderSearchCondition(folderObject) {
			IncludeSubDirectories = includeSubDirectory
		});
	}
}
