using PixChest.Utils.Enums;

namespace PixChest.Models.Preferences.CustomConfigs.Objects;

public class ExtensionConfig {
	public ExtensionConfig() {
	}
	public ExtensionConfig(string extension, MediaType mediaType) {
		this.Extension.Value = extension;
		this.MediaType.Value = mediaType;
	}
	public ReactiveProperty<string> Extension {
		get;
		set;
	} = new();

	public ReactiveProperty<MediaType> MediaType {
		get;
		set;
	} = new();
}
