EacToGracenote
=======

EACからGracenoteへ接続するためのCDDB中継サーバ

## これは何？
Exact Audio Copy (EAC)のfreedb Metadata Pluginから、間接的にGracenoteのCD情報を取得するためのサーバです。CD情報は取得にのみ対応し、登録や更新はできません。

## デモサイト
[http://gncddb.azurewebsites.net/](http://gncddb.azurewebsites.net/)

## 使い方(サーバ)
あらかじめ[Gracenoteの開発者登録](https://developer.gracenote.com/)を行ってください。
必要なものはアプリケーションのClient IDです。
アプリケーションのClient IDは登録後のマイページ画面から確認できます。

Client IDを取得後、Web.config内のappSettingsを置き換えて下さい。

サーバはWCFサービスとして動作します。
GracenoteConnector.HostをIISに発行して下さい。
WCFの処理本体はGracenoteConnector.Libraryです。

## 使い方(クライアント)
EACのfreedb Metadata Pluginに以下の値を設定して下さい。
(プロバイダ名に注意してください。Build-in freedb engineでは動作しません)

`freedb server : http://[server-address]/cddb.svc/cddb`

## 詳細
FreeDB形式でTOCを受け取り、そのままGracenoteWebAPIに受け渡しているだけです。
WCFの処理はGracenoteConnector.Library.CddbService.Cddbメソッドを参照して下さい。
