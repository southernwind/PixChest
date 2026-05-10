using GenJsonConfig.Attributes;
using MediaDeck.Composition.Stores.Config.Model.Objects;

namespace MediaDeck.Composition.Stores.Config.Model;

/// <summary>
/// フォルダ管理設定
/// </summary>
[Inject(InjectServiceLifetime.Singleton)]
[GenerateJsonConfigDto]
public class FolderManagerConfigModel {
	/// <summary>
	/// 管理対象フォルダリスト
	/// </summary>
	public ObservableList<FolderModel> Folders {
		get;
	} = [];
}