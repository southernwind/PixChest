using PixChest.Models.Preferences.CustomConfigs.Objects;

namespace PixChest.Models.Preferences.CustomConfigs;

[AddSingleton]
public class ExecutionConfig : SettingsBase {
	/// <summary>
	/// 実行プログラム
	/// </summary>

	public SettingsCollection<ExecutionProgramObject> ExecutionProgramObjects {
		get;
	} = new ([]) {
		MaybeEditMember = true
	};
}
