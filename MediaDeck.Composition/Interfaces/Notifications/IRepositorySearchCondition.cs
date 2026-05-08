using MediaDeck.Composition.Interfaces.Files;

namespace MediaDeck.Composition.Interfaces.Notifications;

/// <summary>
/// リポジトリ（フォルダやアルバム）に基づく検索条件であることを示すマーカーインターフェース。
/// 検索条件の排他制御（フォルダとアルバムを同時に有効にしないなど）に使用されます。
/// </summary>
public interface IRepositorySearchCondition : ISearchCondition {
}