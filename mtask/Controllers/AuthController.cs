using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using mtask.Lib;
using mtask.Models.DomainModel;
using mtask.Models.ViewModel;
using mtask.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;

namespace mtask.Controllers
{
    /// <summary>
    /// 認証コントローラ
    /// </summary>
    public class AuthController : ApplicationController
    {
        protected readonly IUserService userService = null;

        [InjectionConstructor]
        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        public IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;
        public UserManager<User> UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        public SignInManager<User, string> SignInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        /// <summary>
        /// ログオン画面表示
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logon(string returnUrl)
        {
            var action = "Logon";

            ControllerUtil.MergeTempData(this);
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        /// <summary>
        /// ログオン処理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <param name="defaultAction"></param>
        /// <param name="defaultController"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logon(LogonViewModel model, string returnUrl,
            string defaultAction = "Index",
            string defaultController = "Home")
        {
            var action = "Logon";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action);
            }

            var status = await userService.Logon(SignInManager, ModelState, TempData, model);
            if (status != SignInStatus.Success)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(defaultAction, defaultController);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLogon(ExternalLogonListViewModel model, string returnUrl,
            string defaultAction = "Index",
            string defaultController = "Home")
        {
            var action = "ExternalLogon";

            throw new NotImplementedException();
        }

        /// <summary>
        /// ログオフ処理
        /// </summary>
        /// <param name="defaultAction"></param>
        /// <param name="defaultController"></param>
        /// <returns></returns>
        public ActionResult Logoff(string defaultAction = "Index", string defaultController = "Home")
        {
            var action = "Logoff";

            userService.Logoff(SignInManager);

            return RedirectToAction(defaultAction, defaultController);
        }

        /// <summary>
        /// 登録画面表示
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            var action = "Register";

            ControllerUtil.MergeTempData(this);

            return View();
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var action = "Register";

            if (!ModelState.IsValid)
                return View(model);

            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("DisplayEmail");
            }
            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            var action = "ConfirmEmail";

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return View("Error");

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}