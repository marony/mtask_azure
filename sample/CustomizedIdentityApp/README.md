ASP.NETカスタマイズ
=====

## インストールするNuGetパッケージ
- Microsoft.AspNet.Identity.Owin  
  ASP.NET Identityのコア部分を使用するためにインストールする
- Microsoft.Owin.Host.SystemWeb  
  OwinをIISでホストするためにインストールする
- Microsoft.AspNet.FriendlyUrls  
  管理URLルーティングを行うためにインストールする

## 実装するクラス
- Startup  
  Owinミドルウェアの登録を行う。  
  ASP.NET Identityに関するOwinミドルウェアを登録する。
- ApplicationUser : IUser&lt;out TKey&gt;  
  ユーザー情報を保持する。
- ApplicationUserStore : IUserStore&lt;TUser&gt;, IUserPasswordStore&lt;TUser&gt;  
  ユーザー、パスワードの永続化に関する処理を行う。
- ApplicationSignInManager : SignInManager
  ログイン処理を行う。
