# GitHub Copilot Instructions for MediaDeck

このリポジトリにおけるコーディング規約、アーキテクチャ、および作業手順は [AGENTS.md](../AGENTS.md) に定義されています。
コード生成、編集、レビューを行う際は、AGENTS.md に記載されている以下のルールを必ず遵守してください。

## 主な遵守事項
1. **言語・フレームワーク**: C# (.NET 10), WinUI 3 (Windows App SDK), SQLite (EF Core)
2. **リアクティブ・状態管理 (R3)**:
   - 非同期処理・コマンド・データバインディングには `R3` を使用する（`ReactiveProperty`, `ReactiveCommand` 等）。
   - すべての購読・ReactivePropertyは必ず `AddTo(this.CompositeDisposable)` すること。
   - 非同期購読には必ず `SubscribeAwait` を使用すること（`Subscribe(async ...)` は禁止）。
3. **MVVM & Dispose規約**:
   - ViewModel/Model/Service は `DisposableBase` 派生クラスを継承すること。
   - View (XAML) では `{Binding}` ではなく `{x:Bind}` を優先すること。
4. **国際化 (i18n)**:
   - UI文字列はハードコードせず `Strings/ja/Resources.resw` および `Strings/en/Resources.resw` に定義すること。
   - ロジック内では `IStringProvider` を使用すること。
5. **バージョンアップ・リリース手順**:
   - バージョン更新作業を行う際は [.agents/skills/version-bump/SKILL.md](../.agents/skills/version-bump/SKILL.md) または AGENTS.md の手順に従うこと。
