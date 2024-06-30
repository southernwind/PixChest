using System.Collections.Generic;
using PixChest.Composition.Bases;
using PixChest.Models.Files.Filter.FilterItemObjects;
using PixChest.Models.FilesFilter;
using PixChest.ViewModels.Filters.Creators;

namespace PixChest.ViewModels.Filters;

public class FilteringConditionViewModel : ViewModelBase {
	/// <summary>
	/// モデル
	/// </summary>
	public FilteringCondition Model {
		get;
	}

	/// <summary>
	/// 表示名
	/// </summary>
	public BindableReactiveProperty<string> DisplayName {
		get;
	}

	/// <summary>
	/// フィルター条件クリエイター
	/// </summary>
	public Reactive.Bindings.ReadOnlyReactiveCollection<IFilterItemObject> FilterItems {
		get;
	}

	/// <summary>
	/// フィルター条件作成VMリスト
	/// </summary>
	public IEnumerable<IFilterCreatorViewModel> FilterCreatorViewModels {
		get;
	}

	/// <summary>
	/// 選択中フィルター条件作成VM
	/// </summary>
	public BindableReactiveProperty<IFilterCreatorViewModel> SelectedFilterCreatorViewModel {
		get;
	} = new();

	/// <summary>
	/// フィルター削除コマンド
	/// </summary>
	public ReactiveCommand<IFilterItemObject> RemoveFilterCommand {
		get;
	} = new ReactiveCommand<IFilterItemObject>();


	public FilteringConditionViewModel(FilteringCondition model) {
		this.Model = model;
		this.DisplayName = this.Model.DisplayName.ToBindableReactiveProperty(null!);

		this.FilterItems = Reactive.Bindings.ReadOnlyReactiveCollection.ToReadOnlyReactiveCollection(this.Model.FilterItemObjects);

		this.FilterCreatorViewModels = [
				new ExistsFilterCreatorViewModel(model),
				new FilePathFilterCreatorViewModel(model),
				new LocationFilterCreatorViewModel(model),
				new MediaTypeFilterCreatorViewModel(model),
				new RateFilterCreatorViewModel(model),
				new ResolutionFilterCreatorViewModel(model),
				new TagFilterCreatorViewModel(model)
			];
		this.SelectedFilterCreatorViewModel.Value = this.FilterCreatorViewModels.First();

		// 削除
		this.RemoveFilterCommand.Subscribe(this.Model.RemoveFilter).AddTo(this.CompositeDisposable);
	}
}
