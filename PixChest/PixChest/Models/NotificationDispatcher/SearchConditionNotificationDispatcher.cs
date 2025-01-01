using PixChest.Models.Files.SearchConditions;

namespace PixChest.Models.NotificationDispatcher;
[AddSingleton]
public class SearchConditionNotificationDispatcher {
	public Subject<ISearchCondition> AddRequest {
		get;
	} = new();

	public Subject<ISearchCondition> RemoveRequest {
		get;
	} = new();

	public Subject<Action<ObservableList<ISearchCondition>>> UpdateRequest {
		get;
	} = new();
}
