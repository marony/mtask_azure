using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Models;
using mtask.Models.DomainModel;
using mtask.Models.ViewModel;
using mtask.Services;
using Microsoft.Azure;
using Microsoft.Practices.Unity;

namespace mtask.Controllers
{
    /// <summary>
    /// Homeコントローラ
    /// </summary>
    [Authorize]
    public class HomeController : ApplicationController
    {
        protected readonly IUserService userService = null;

        [InjectionConstructor]
        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Home画面表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = userService.GetCurrentUser(this.User);
            var model = new HomeIndexViewModel() { User = user };

            return View(model);
        }
    }
}