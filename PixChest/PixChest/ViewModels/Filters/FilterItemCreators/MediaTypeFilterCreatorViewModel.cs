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
	public ReactiveCommand AddMediaTypeFilterCommand {
		get;
	} = new ReactiveCommand();

	/// <summary>
	/// メディアタイプ
	/// </summary>
	public IReactiveProperty<DisplayObject<bool>> MediaType {
		get;
	} = new ReactivePropertySlim<DisplayObject<bool>>();

	/// <summary>
	/// メディアタイプ候補
	/// </summary>
	public IEnumerable<DisplayObject<bool>> MediaTypeList {
		get;
	} = [
		new DisplayObject<bool>("画像",false),
		new DisplayObject<bool>("動画",true)
	];

	public MediaTypeFilterCreatorViewModel(FilteringCondition model) {
		this.MediaType.Value = this.MediaTypeList.First();
		this.AddMediaTypeFilterCommand.Subscribe(() => model.AddMediaTypeFilter(this.MediaType.Value.Value));

	}
}
