using MediaDeck.Common.Base;
using MediaDeck.Common.Extensions;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Composition.Stores.Config.Model;

namespace MediaDeck.ViewModels.Preferences.Config;

[Inject(InjectServiceLifetime.Transient)]
public class SearchConfigPageViewModel : ViewModelBase, IConfigPageViewModel {
	/// <summary>
	/// ページ名
	/// </summary>
	public string PageName {
		get;
	}

	/// <summary>
	/// ページのアイコン（Segoe Fluent Icons のグリフ文字）
	/// </summary>
	public string PageIconGlyph {
		get;
	} = "\uE71E";

	/// <summary>
	/// ページの説明
	/// </summary>
	public string PageDescription {
		get;
	}


	public SearchConfigPageViewModel(SearchConfigModel searchConfig, IStringProvider stringProvider) {
		this.PageName = stringProvider.GetString("Config_Search_Name");
		this.PageDescription = stringProvider.GetString("Config_Search_Description");

		this.InitialLoadCount = searchConfig.InitialLoadCount.ToTwoWayBindableReactiveProperty(500, this.CompositeDisposable).AddTo(this.CompositeDisposable);
		this.IncrementalLoadCount = searchConfig.IncrementalLoadCount.ToTwoWayBindableReactiveProperty(10000, this.CompositeDisposable).AddTo(this.CompositeDisposable);
		this.MaxLoadCount = searchConfig.MaxLoadCount.ToTwoWayBindableReactiveProperty(50000, this.CompositeDisposable).AddTo(this.CompositeDisposable);
	}

	/// <summary>
	/// 初期ロード件数
	/// </summary>
	public BindableReactiveProperty<int> InitialLoadCount {
		get;
	}

	/// <summary>
	/// 増分読み込み件数
	/// </summary>
	public BindableReactiveProperty<int> IncrementalLoadCount {
		get;
	}

	/// <summary>
	/// 最大件数
	/// </summary>
	public BindableReactiveProperty<int> MaxLoadCount {
		get;
	}
}