using System;
using System.Web;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CustomizedIdentityApp
{
  public partial class Site : System.Web.UI.MasterPage
  {
    protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
    {
      // ログアウトする
      var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
      authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
    }
  }
}