using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomizedIdentityApp.Startup))]
namespace CustomizedIdentityApp
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // リクエストごとにOwinContextにApplicationUserManagerインスタンスを作成して登録する
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      // Cookie認証を行う
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString("/Login")
      });
    }
  }
}
