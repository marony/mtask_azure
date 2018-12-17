using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CustomizedIdentityApp.Tests
{
  [TestClass]
  public class ApplicationUserManagerTest
  {
    private ApplicationUserManager manager;

    [TestInitialize]
    public void SetUp()
    {
      var options = new IdentityFactoryOptions<ApplicationUserManager>();
      var context = new OwinContext();
      manager = ApplicationUserManager.Create(options, context);
    }

    [TestCleanup]
    public void TearDown()
    {
      manager.Dispose();
    }

    [TestMethod]
    public void ユーザー情報を検索する()
    {
      var user = manager.Find("takano-s", "123456");
      user.UserName.Is("takano-s");
    }

    [TestMethod]
    public void ユーザー情報の検索に失敗する()
    {
      var user = manager.Find("takano-s", "xxx");
      user.IsNull();
    }

    [TestMethod]
    public void 識別情報を作成する()
    {
      var user = new ApplicationUser(Guid.NewGuid().ToString())
      {
        UserName = "takano-s"
      };
      var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
      userIdentity.GetUserName().Is("takano-s");
    }
  }
}
