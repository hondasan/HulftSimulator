# HULFT 定期送信シミュレータ

Windows 上で HULFT のファイル送信を指定周期で疑似実行できる WinForms アプリです。HULFT がインストールされていない端末でも動作し、送信部分は `IHulftSender` インターフェースを介して差し替え可能なスタブとして実装しています。

## 動作概要
- HULFT ファイル ID、送信フォルダ、開始ファイル、送信周期（秒）を UI から設定
- フォルダ選択後に `yyyymmddHHMM.csv` 形式のファイルのみを昇順で読み込み、開始ファイルをプルダウン選択
- 指定周期ごとに 1 ファイルずつ送信（開始ファイル以降を順番に処理）
- 送信結果をリアルタイムに UI 表示＋ `logs/hulft_yyyyMMdd.log` へ追記
- HULFT が見つからない場合は「HULFT未インストール」をログ出力してスキップ
- 送信対象がなくなると自動停止。ユーザーは随時開始／停止可能

## 主要プロジェクト構成
```
HulftScheduler.sln
└─ HulftSchedulerApp (WinForms, .NET Framework 4.8)
   ├─ Program.cs                : エントリポイント
   ├─ MainForm / Designer       : UI と送信制御ロジック
   ├─ Logging/LogService.cs     : UI＋ファイルへのログ出力
   └─ Services/
        ├─ IHulftSender.cs      : 送信インターフェース
        ├─ HulftSenderFactory   : 環境に応じて送信クラスを選択
        ├─ RealHulftSender      : HULFT 有り前提のスタブ実装
        └─ DummyHulftSender     : HULFT 無し時のスキップ実装
```

## ビルドと実行
1. Windows + Visual Studio 2019 で `HulftScheduler.sln` を開く
2. 対象フレームワークは **.NET Framework 4.8**
3. F5 実行、または `HulftSchedulerApp` をスタートアップにしてビルド

> Linux 上の dotnet CLI では .NET Framework プロジェクトをビルドできないため、必ず Windows 環境でビルドしてください。

## 送信スタブの切り替え
`RealHulftSender.IsHulftInstalled()` が以下のいずれかで真になると Real 版を利用します。
- `HULFT_INSTALLED` 環境変数に `1` を設定
- `C:\Program Files\HULFT` ディレクトリが存在する

上記以外の環境では `DummyHulftSender` が選択され、「HULFT未インストールのため送信をスキップ」とログに出力されます。実際の HULFT 連携ロジックは `RealHulftSender.SendFile` 内に実装してください。

## 実行例ログ
```
[2025-01-01 10:00:00] HULFT未インストールのため DummyHulftSender で送信をスキップします。
[2025-01-01 10:00:05] 送信開始 (ファイルID: SALES001, フォルダ: C:\data\hulft, 開始ファイル: 202501011000.csv, 周期: 30秒)
[2025-01-01 10:00:05] 202501011000.csv → 送信失敗 (HULFT未インストールのため送信をスキップ)
[2025-01-01 10:00:35] 202501011030.csv → 送信失敗 (HULFT未インストールのため送信をスキップ)
[2025-01-01 10:01:05] 送信対象ファイルがなくなったため処理を終了します。
```

## 改善ポイント（例）
- 送信失敗時のリトライ回数やバックオフ制御を設定できるようにする
- フォルダ監視機能を追加して、リアルタイムに新規ファイルをキューへ追加
- `IHulftSender` の実装を外部 DLL としてプラグイン化し、環境ごとに差し替え可能にする
- NLog 等の本格的なロガーへ差し替え、ログローテーションやレベル管理を行う
