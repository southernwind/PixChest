using System.ComponentModel;

using PixChest.Composition.Bases;
using PixChest.Models.Files.Sort;

namespace PixChest.ViewModels.Sort;

public class SortConditionEditorViewModel : ViewModelBase {
	public SortConditionEditorViewModel(SortConditionEditor model) {
		this.Model = model;
		this.DisplayName = this.Model.DisplayName.ToBindableReactiveProperty(null!);
		this.DisplayName.Subscribe(x => {
			this.Model.DisplayName.Value = x;
		});

		this.SortItemCreators = this.Model.SortItemCreators.ToNotifyCollectionChanged(SynchronizationContextCollectionEventDispatcher.Current);

		this.AddSortItemCommand.Subscribe(this.Model.AddSortItem).AddTo(this.CompositeDisposable);
		this.RemoveSortItemCommand.Subscribe(this.Model.RemoveSortItem).AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// モデル
	/// </summary>
	public SortConditionEditor Model {
		get;
	}

	/// <summary>
	/// 表示名
	/// </summary>
	public BindableReactiveProperty<string> DisplayName {
		get;
	}

	/// <summary>
	/// ソート条件クリエイター
	/// </summary>
	public INotifyCollectionChangedSynchronizedViewList<SortItemCreator> SortItemCreators {
		get;
	}

	/// <summary>
	///　設定用ソート項目リスト
	/// </summary>
	public ReactiveProperty<SortItemCreator[]> CandidateSortItemCreators {
		get;
	} = new ();

	/// <summary>
	/// 設定用選択中ソート項目
	/// </summary>
	public ReactiveProperty<SortItemCreator> SelectedSortItemCreator {
		get;
	} = new ();

	/// <summary>
	/// 設定用ソート方向
	/// </summary>
	public BindableReactiveProperty<ListSortDirection> Direction {
		get;
	} = new (ListSortDirection.Ascending);

	/// <summary>
	/// ソート条件削除コマンド
	/// </summary>
	public ReactiveCommand<SortItemCreator> RemoveSortItemCommand {
		get;
	} = new ReactiveCommand<SortItemCreator>();

	/// <summary>
	/// ソート条件削除コマンド
	/// </summary>
	public ReactiveCommand<SortItemCreator> AddSortItemCommand {
		get;
	} = new ReactiveCommand<SortItemCreator>();
}
