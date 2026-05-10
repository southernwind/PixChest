using MediaDeck.Common.Base;
using MediaDeck.Composition.Enum;
using MediaDeck.Composition.Interfaces;
using MediaDeck.Composition.Stores.Config.Model;

namespace MediaDeck.ViewModels.Preferences.Config;

[Inject(InjectServiceLifetime.Transient)]
public class EnvironmentConfigPageViewModel : ViewModelBase, IConfigPageViewModel {
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
	} = "\uF4C4";

	/// <summary>
	/// ページの説明
	/// </summary>
	public string PageDescription {
		get;
	}

	/// <summary>
	/// テーマ選択肢
	/// </summary>
	public ThemeOption[] ThemeOptions {
		get;
	}

	/// <summary>
	/// 選択中のテーマ
	/// </summary>
	public BindableReactiveProperty<ThemeOption> SelectedTheme {
		get;
	}

	public EnvironmentConfigPageViewModel(EnvironmentConfigModel environmentConfig, IStringProvider stringProvider) {
		this.PageName = stringProvider.GetString("Config_Environment_Name");
		this.PageDescription = stringProvider.GetString("Config_Environment_Description");

		this.ThemeOptions = [
			new ThemeOption { Theme = AppTheme.Default, DisplayName = stringProvider.GetString("Config_Environment_Theme_Default") },
			new ThemeOption { Theme = AppTheme.Light, DisplayName = stringProvider.GetString("Config_Environment_Theme_Light") },
			new ThemeOption { Theme = AppTheme.Dark, DisplayName = stringProvider.GetString("Config_Environment_Theme_Dark") },
		];

		var currentTheme = environmentConfig.Theme.Value;
		var initial = Array.Find(this.ThemeOptions, x => x.Theme == currentTheme)
					  ?? this.ThemeOptions[0];

		this.SelectedTheme = new BindableReactiveProperty<ThemeOption>(initial);

		// ViewModel の選択変更を Model に反映
		this.SelectedTheme
			.Subscribe(opt => environmentConfig.Theme.Value = opt?.Theme ?? AppTheme.Default)
			.AddTo(this.CompositeDisposable);
	}
}

/// <summary>
/// テーマの選択肢
/// </summary>
public sealed class ThemeOption {
	public AppTheme Theme {
		get;
		init;
	}

	public string DisplayName {
		get;
		init;
	} = string.Empty;

	public override string ToString() {
		return this.DisplayName;
	}
}