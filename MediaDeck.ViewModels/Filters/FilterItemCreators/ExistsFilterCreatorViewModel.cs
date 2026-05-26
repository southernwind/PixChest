using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Core.Models.Files.Filter.FilterItemObjects;
using MediaDeck.Core.Primitives;

namespace MediaDeck.ViewModels.Filters.FilterItemCreators;

/// <summary>
/// 存在フィルター作成ViewModel
/// </summary>
public class ExistsFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	private readonly IStringProvider _stringProvider;

	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return this._stringProvider.GetString("FilterCreator_Exists_Title");
		}
	}

	/// <summary>
	/// ファイルが存在するか否か
	/// </summary>
	public BindableReactiveProperty<DisplayObject<bool>> Exists {
		get;
	} = new();

	/// <summary>
	/// ファイルが存在するか否かの候補
	/// </summary>
	public IEnumerable<DisplayObject<bool>> ExistsList {
		get;
	}

	/// <summary>
	/// フィルター追加コマンド
	/// </summary>
	public ReactiveCommand AddFilterCommand {
		get;
	} = new();

	public ExistsFilterCreatorViewModel(ReactiveProperty<FilteringConditionEditorViewModel?> target, IStringProvider stringProvider) {
		this._stringProvider = stringProvider;
		this.ExistsList = [
			new DisplayObject<bool>(this._stringProvider.GetString("FilterCreator_Exists_True"), true),
			new DisplayObject<bool>(this._stringProvider.GetString("FilterCreator_Exists_False"), false)
		];
		this.Exists.Value = this.ExistsList.First();
		this.AddFilterCommand.Subscribe(vm => {
			var filter = new ExistsFilterItemObject {
				Exists = this.Exists.Value.Value
			};
			target.Value?.AddFilter(filter);
		}).AddTo(this.CompositeDisposable);
	}
}