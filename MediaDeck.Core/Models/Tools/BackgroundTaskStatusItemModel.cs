using MediaDeck.Common.Base;

namespace MediaDeck.Core.Models.Tools;

/// <summary>
/// バックグラウンドタスク項目Model。
/// </summary>
public class BackgroundTaskStatusItemModel : ModelBase {
	/// <summary>
	/// バックグラウンドタスク項目Modelを初期化する。
	/// </summary>
	/// <param name="displayName">表示名</param>
	/// <param name="completedCount">完了件数</param>
	/// <param name="targetCount">対象件数</param>
	/// <param name="reRun">再実行処理</param>
	/// <param name="cancel">キャンセル処理</param>
	public BackgroundTaskStatusItemModel(string displayName, ReactiveProperty<long> completedCount, ReactiveProperty<long> targetCount, Action reRun, Action? cancel = null) {
		this.DisplayName = displayName;
		this.CompletedCount = completedCount;
		this.TargetCount = targetCount;
		this.ReRun = reRun;
		this.Cancel = cancel;
	}

	/// <summary>
	/// 表示名
	/// </summary>
	public string DisplayName {
		get;
	}

	/// <summary>
	/// 完了件数
	/// </summary>
	public ReactiveProperty<long> CompletedCount {
		get;
	}

	/// <summary>
	/// 対象件数
	/// </summary>
	public ReactiveProperty<long> TargetCount {
		get;
	}

	/// <summary>
	/// 再実行処理
	/// </summary>
	public Action ReRun {
		get;
	}

	/// <summary>
	/// キャンセル処理
	/// </summary>
	public Action? Cancel {
		get;
	}
}