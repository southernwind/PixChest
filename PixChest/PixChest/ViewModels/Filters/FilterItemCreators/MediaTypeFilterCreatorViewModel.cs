using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Models.FilesFilter;
using PixChest.Utils.Objects;

namespace PixChest.ViewModels.Filters.Creators;
/// <summary>
/// メディアタイプフィルター作成ViewModel
/// </summary>
public class MediaTypeFilterCreatorViewModel : ViewModelBase, IFilterCreatorViewModel {
	/// <summary>
	/// 表示名
	/// </summary>
	public string Title {
		get {
			return "メディアタイプフィルター";
		}
	}

	/// <summary>
	/// メディアタイプフィルター追加コマンド
	/// </summary>
	public ReactiveCommand<Unit> AddMediaTypeFilterCommand {
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
	} = [
		new DisplayObject<bool>("画像",false),
		new DisplayObject<bool>("動画",true)
	];

	public MediaTypeFilterCreatorViewModel(FilteringConditionEditor model) {
		this.MediaType.Value = this.MediaTypeList.First();
		this.AddMediaTypeFilterCommand.Subscribe(_ => model.AddMediaTypeFilter(this.MediaType.Value.Value));

	}
}
