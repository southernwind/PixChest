using PixChest.Utils.Enums;

namespace PixChest.Models.Preferences.CustomConfig.Objects;

public class ExtensionConfig {
	public ExtensionConfig() {
	}
	public ExtensionConfig(string extension, MediaType mediaType) {
		this.Extension.Value = extension;
		this.MediaType.Value = mediaType;
	}
	public IReactiveProperty<string> Extension {
		get;
		set;
	} = new ReactiveProperty<string>();

	public IReactiveProperty<MediaType> MediaType {
		get;
		set;
	} = new ReactiveProperty<MediaType>();
}
