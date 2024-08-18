using System.IO;
using System.Threading.Tasks;

using PixChest.Database;
using PixChest.Models.Preferences;
using PixChest.Models.Repositories.Objects;
using PixChest.Utils.Objects;

namespace PixChest.Models.Repositories;

[AddTransient]
public class FolderRepository(PixChestDbContext dbContext,States states): RepositoryBase {
	private readonly PixChestDbContext _db = dbContext;
	private readonly States _states = states;

	public ReactiveProperty<FolderObject> RootFolder {
		get;
	} = new();

	public override async Task Load() {
		var list = (await this._db
			.MediaFiles
			.GroupBy(x => x.DirectoryPath)
			.Select(x => new ValueCountPair<string>(x.Key, x.Count()))
			.ToListAsync())
			.OrderBy(x => x.Value)
			.ToList();

		var all = list.Select(x => (x.Value, x.Count, Split: x.Value.Split(Path.DirectorySeparatorChar))).ToArray();
		var maxPathDepth = all.Max(x => x.Split.Length);
		
		// ルート追加
		list.Add(new ("",list.Sum(x => x.Count)));
		// 足りない部分を補足
		for (var depth = 1; depth <= maxPathDepth; depth++) { // 最大の深さまで
			var target = all.Where(x => x.Split.Length > depth).ToArray(); // 深さが足りないものを除外
			list.AddRange(target
				.Select(x =>
					(
						item: x,
						joined: string.Join("\\", x.Split[0..depth]), // 対象の深さまでのパス
						joinedPlus: string.Join("\\", x.Split[0..(depth + 1)]) // 対象の深さ+1までのパス
					)
				)
				.Where(x => !all.Any(y => y.Value == x.joined)) // 対象のパスがすでに含まれていたら除外
				.GroupBy(x => x.joined) // 対象の深さまでのパスでグループ化
				.Where(x => x.DistinctBy(x => x.joinedPlus).Count() >= 2) // 対象の深さから、2つ以上のフォルダに分岐している
				.Select(x => new ValueCountPair<string>(x.Key, 0)));
		}

		this.RootFolder.Value = new FolderObject(null, "", [.. list.OrderBy(x => x.Value)]);
	}

	public void SetRepositoryCandidate(FolderObject folderObject) {
		var rco = new FolderRepositoryConditionObject() {
			DirectoryPath = folderObject.FolderPath
		};
		this._states.SearchStates.CurrentRepositoryCondition.Value = rco;
	}
}
