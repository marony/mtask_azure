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
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace mtask.Controllers
{
    /// <summary>
    /// ストーリーコントローラ
    /// </summary>
    [Authorize]
    public class StoryController : ApplicationController
    {
        protected readonly IUserService userService = null;
        protected readonly IProjectService projectService = null;

        [InjectionConstructor]
        public StoryController(IUserService userService, IProjectService projectService)
        {
            this.userService = userService;
            this.projectService = projectService;
        }

        /// <summary>
        /// ストーリー一覧
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
        /// ストーリー追加画面表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(string projectId, string returnUrl)
        {
            var action = "Add";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var user = userService.GetCurrentUser(User);
            var project = projectService.GetProject(user, projectId);
            if (project == null)
                return RedirectToAction("Index");

            var model = new StoryAddInputModel() { Project = project, ProjectId = project.Id };
            return View(model);
        }

        /// <summary>
        /// ストーリー追加処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(StoryAddInputModel model, string returnUrl)
        {
            var action = "Add";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ProjectId = model.ProjectId, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var story = projectService.AddStory(ModelState, user, model.ProjectId, model.Description);
            if (story == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ProjectId = model.ProjectId, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Project", new { Id = model.ProjectId });
            }
        }

        /// <summary>
        /// ストーリー編集画面表示
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
            var story = projectService.GetStory(user, id);
            if (story == null)
                return RedirectToAction("Index");

            var model = new StoryEditInputModel() { Id = id, Description = story.Description };
            return View(model);
        }

        /// <summary>
        /// ストーリー編集処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(StoryEditInputModel model, string returnUrl)
        {
            var action = "Edit";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var story = projectService.EditStory(ModelState, user, model.Id, model.Description);
            if (story == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }
            else
            {
                var project = story.Project ?? story.Sprint.Project;
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Project", new { Id = project.Id });
            }
        }

        /// <summary>
        /// ストーリー削除画面表示
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
            var story = projectService.GetStory(user, id);
            if (story == null)
                return RedirectToAction("Index");

            var project = story.Project ?? story.Sprint.Project;
            var sprint = story.Sprint;
            var model = new StoryDeleteInputModel() { Project = project, ProjectId = project.Id, Sprint = sprint, Id = id, Description = story.Description };
            return View(model);
        }

        /// <summary>
        /// ストーリー削除処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(StoryDeleteInputModel model, string returnUrl)
        {
            var action = "Delete";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var story = projectService.DeleteStory(ModelState, user, model.Id);
            if (story == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Project", new { Id = model.ProjectId });
            }
        }

        /// <summary>
        /// ストーリー(の中身)表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Show(string id)
        {
            var action = "Show";

            var user = userService.GetCurrentUser(this.User);

            var story = projectService.GetStory(user, id);
            if (story == null)
                return RedirectToAction("Index");

            var project = story.Project ?? story.Sprint.Project;
            var model = new StoryShowViewModel() { Project = project, Sprint = story.Sprint, Story = story };
            return View(model);
        }
    }
}
