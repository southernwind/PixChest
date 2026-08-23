---
name: version-bump
description: >-
  新バージョンのリリース準備、バージョン番号の更新、CHANGELOG.mdの更新、
  またはバージョンコミットを行う際に使用する。
---

# バージョンアップ & リリース準備手順

新バージョンのリリース準備、バージョン番号更新、CHANGELOG更新、およびバージョンコミットを行う際の手順書です。

## 1. コミットログと前回タグの確認

1. 前回タグを確認する:
   ```bash
   git tag --sort=-version:refname
   ```
2. 前回タグからのコミットログを取得する:
   ```bash
   git log <前回タグ>..HEAD --oneline --no-merges
   ```

## 2. バージョン番号の更新
以下の3箇所のバージョン番号を新バージョンに更新します。

1. **[Directory.Build.props](file:///Directory.Build.props)**
   - `<Version>`: 新バージョン（例: `1.0.3`）
   - `<AssemblyVersion>`: 新バージョン（例: `1.0.3.0`）
   - `<FileVersion>`: 新バージョン（例: `1.0.3.0`）
2. **[MediaDeck/Package.appxmanifest](file:///MediaDeck/Package.appxmanifest)**
   - `<Identity Version="...">`: 新バージョン（4桁形式、例: `1.0.3.0`）
3. **[MediaDeck/Package.Dev.appxmanifest](file:///MediaDeck/Package.Dev.appxmanifest)**
   - `<Identity Version="...">`: 新バージョン（4桁形式、例: `1.0.3.0`）
   - ※ Debug ビルドで使われるため忘れずに更新すること。

## 3. CHANGELOG.md の更新

[CHANGELOG.md](file:///CHANGELOG.md) の先頭に新バージョンのエントリを追加します。

### CHANGELOG の記載ルール
- **対象読者**: 一般ユーザー向け。
- **記載基準**: ユーザーの操作・体験に直接影響する変更のみを記載する。内部的なリファクタリング、コード品質、開発用スクリプトの変更などは記載しない。
- **文体**: 技術用語・クラス名・メソッド名は使わず、「〜できるようになりました」「〜する問題を修正しました」のように平易な日本語で書く。

## 4. ビルド・テスト・フォーマットの検証

作業完了前に、ビルドエラー、テスト失敗、フォーマット違反がないことを確認します。

```bash
dotnet build
dotnet test
dotnet format --verify-no-changes
```

## 5. コミットの作成

検証に合格したら、更新した設定ファイルと CHANGELOG.md をコミットします。

- **コミットメッセージ形式**: `vX.Y.Z`（例: `v1.0.3`）
