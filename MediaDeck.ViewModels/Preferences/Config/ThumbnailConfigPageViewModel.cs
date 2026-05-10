using MediaDeck.Common.Base;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Composition.Stores.Config.Model;

namespace MediaDeck.ViewModels.Preferences.Config;

/// <summary>
/// サムネイル設定ページのViewModel
/// </summary>
[Inject(InjectServiceLifetime.Transient)]
public class ThumbnailConfigPageViewModel : ViewModelBase, IConfigPageViewModel {
	/// <inheritdoc/>
	public string PageName {
		get;
	}

	/// <inheritdoc/>
	public string PageIconGlyph {
		get;
	} = "\uEB9F"; // Photo2 icon

	/// <inheritdoc/>
	public string PageDescription {
		get;
	}

	private readonly ThumbnailConfigModel _thumbnailConfig;

	public ThumbnailConfigPageViewModel(ThumbnailConfigModel thumbnailConfig, IStringProvider stringProvider) {
		this._thumbnailConfig = thumbnailConfig;
		this.PageName = stringProvider.GetString("Config_Thumbnail_Name");
		this.PageDescription = stringProvider.GetString("Config_Thumbnail_Description");
	}

	/// <summary>
	/// サムネイル作成サイズ
	/// </summary>
	public ReactiveProperty<int> ThumbnailSize {
		get {
			return this._thumbnailConfig.ThumbnailSize;
		}
	}
}