using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Core.Models.Files.Filter.FilterItemObjects;
using MediaDeck.Core.Primitives;

namespace MediaDeck.ViewModels.Filters.FilterItemCreators;

/// <summary>
/// メディアタイプフィルター作成ViewModel
/// </summary>
public class MediaTypeFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	private readonly IStringProvider _stringProvider;

	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return this._stringProvider.GetString("FilterCreator_MediaType_Title");
		}
	}

	/// <summary>
	/// フィルター追加コマンド
	/// </summary>
	public ReactiveCommand AddFilterCommand {
		get;
	} = new();

	/// <summary>
	/// メディアタイプ
	/// </summary>
	public BindableReactiveProperty<DisplayObject<bool>> MediaType {
		get;
	} = new();

	/// <summary>
	/// メディアタイプ候補
	/// </summary>
	public IEnumerable<DisplayObject<bool>> MediaTypeList {
		get;
	}

	public MediaTypeFilterCreatorViewModel(ReactiveProperty<FilteringConditionEditorViewModel?> target, IStringProvider stringProvider) {
		this._stringProvider = stringProvider;
		this.MediaTypeList = [
			new DisplayObject<bool>(this._stringProvider.GetString("FilterCreator_MediaType_Image"), false),
			new DisplayObject<bool>(this._stringProvider.GetString("FilterCreator_MediaType_Video"), true)
		];
		this.MediaType.Value = this.MediaTypeList.First();
		this.AddFilterCommand.Subscribe(vm => {
			var filter = new MediaTypeFilterItemObject {
				IsVideo = this.MediaType.Value.Value
			};
			target.Value?.AddFilter(filter);
		}).AddTo(this.CompositeDisposable);
	}
}