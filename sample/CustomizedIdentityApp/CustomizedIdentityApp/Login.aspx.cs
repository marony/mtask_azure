using System;
using System.Web;
using System.Web.UI;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CustomizedIdentityApp
{
  public partial class Login : System.Web.UI.Page
  {
    protected void login_Click(object sender, EventArgs e)
    {
      if (Page.IsValid)
      {
        // ユーザー名とパスワードを検証
        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        var user = manager.Find(userName.Text, password.Text);
        if (user == null)
        {
          return;
        }

        // ユーザーの識別情報を作成し、ログインする
        var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        var identity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
        authenticationManager.SignIn(identity);

        // 遷移元画面に遷移する
        var returnUrl = Request.QueryString["ReturnUrl"];
        if(string.IsNullOrEmpty(returnUrl))
        {
          Response.Redirect("~/");
        }
        else
        {
          Response.Redirect(returnUrl);
        }
      }
    }
  }
}