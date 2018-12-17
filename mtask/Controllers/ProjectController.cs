using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Lib;
using mtask.Models;
using mtask.Models.DomainModel;
using mtask.Models.InputModel;
using mtask.Models.ViewModel;
using mtask.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace mtask.Controllers
{
    /// <summary>
    /// プロジェクトコントローラ
    /// </summary>
    [Authorize]
    public class ProjectController : ApplicationController
    {
        protected readonly IUserService userService = null;
        protected readonly IProjectService projectService = null;

        [InjectionConstructor]
        public ProjectController(IUserService userService, IProjectService projectService)
        {
            this.userService = userService;
            this.projectService = projectService;
        }

        /// <summary>
        /// プロジェクト一覧
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var action = "Index";

            User user = userService.GetCurrentUser(this.User);
            var projects = projectService.GetAllProjects(user);
            var model = new ProjectIndexViewModel() { Projects = projects };

            return View(model);
        }

        /// <summary>
        /// プロジェクト追加画面表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(string returnUrl)
        {
            var action = "Add";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var model = new ProjectAddInputModel();
            return View(model);
        }

        /// <summary>
        /// プロジェクト追加処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(ProjectAddInputModel model, string returnUrl)
        {
            var action = "Add";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var project = projectService.AddProject(ModelState, user, model.Id, model.Description);

            if (project == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// プロジェクト編集画面表示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id, string returnUrl)
        {
            var action = "Edit";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var user = userService.GetCurrentUser(User);
            var project = projectService.GetProject(user, id);
            if (project == null)
                return RedirectToAction("Index");

            var model = new ProjectEditInputModel() { Id = project.Id, Description = project.Description };
            return View(model);
        }

        /// <summary>
        /// プロジェクト編集処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ProjectEditInputModel model, string returnUrl)
        {
            var action = "Edit";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var project = projectService.EditProject(ModelState, user, model.Id, model.Description);

            if (project == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// プロジェクト削除画面表示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id, string returnUrl)
        {
            var action = "Delete";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var user = userService.GetCurrentUser(User);
            var project = projectService.GetProject(user, id);
            if (project == null)
                return RedirectToAction("Index");

            var model = new ProjectDeleteInputModel() { Id = project.Id, Description = project.Description };
            return View(model);
        }

        /// <summary>
        /// プロジェクト削除処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(ProjectDeleteInputModel model, string returnUrl)
        {
            var action = "Delete";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var project = projectService.DeleteProject(ModelState, user, model.Id);

            if (project == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// プロジェクト(の中身)表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Show(string id)
        {
            var action = "Show";

            var user = userService.GetCurrentUser(this.User);
            var project = projectService.GetProject(user, id);
            if (project == null)
                return RedirectToAction("Index");

            var model = project;

            return View(model);
        }
    }
}
