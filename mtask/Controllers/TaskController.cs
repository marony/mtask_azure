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
    /// タスクコントローラ
    /// </summary>
    [Authorize]
    public class TaskController : ApplicationController
    {
        protected readonly IUserService userService = null;
        protected readonly IProjectService projectService = null;

        [InjectionConstructor]
        public TaskController(IUserService userService, IProjectService projectService)
        {
            this.userService = userService;
            this.projectService = projectService;
        }

        /// <summary>
        /// タスク一覧
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
        /// タスク追加画面表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add(string storyId, string returnUrl)
        {
            var action = "Add";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var user = userService.GetCurrentUser(User);
            var story = projectService.GetStory(user, storyId);
            if (story == null)
                return RedirectToAction("Index");

            var project = story.Project ?? story.Sprint.Project;
            var model = new TaskAddInputModel() { Story = story, StoryId = story.Id };
            return View(model);
        }

        /// <summary>
        /// タスク追加処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(TaskAddInputModel model, string returnUrl)
        {
            var action = "Add";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { StoryId = model.StoryId, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var task = projectService.AddTask(ModelState, user, model.StoryId, model.Description, model.Point);
            if (task == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { StoryId = model.StoryId, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Story", new { Id = model.StoryId });
            }
        }

        /// <summary>
        /// タスク編集画面表示
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
            var task = projectService.GetTask(user, id);
            if (task == null)
                return RedirectToAction("Index");

            var model = new TaskEditInputModel() { Id = id, Description = task.Description, Point = task.Point, Status = task.Status };
            return View(model);
        }

        /// <summary>
        /// タスク編集処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(TaskEditInputModel model, string returnUrl)
        {
            var action = "Edit";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var task = projectService.EditTask(ModelState, user, model.Id, model.Description, model.Point, model.Status);
            if (task == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Story", new { Id = task.Story.Id });
            }
        }

        /// <summary>
        /// タスク削除画面表示
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
            var task = projectService.GetTask(user, id);
            if (task == null)
                return RedirectToAction("Index");

            var model = new TaskDeleteInputModel() { Story = task.Story, Id = id, Description = task.Description, Point = task.Point };
            return View(model);
        }

        /// <summary>
        /// タスク削除処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(TaskDeleteInputModel model, string returnUrl)
        {
            var action = "Delete";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var task = projectService.DeleteTask(ModelState, user, model.Id);
            if (task == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Story", new { Id = task.Story.Id });
            }
        }

        /// <summary>
        /// タスク(の中身)表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Show(string projectId,　string id)
        {
            var action = "Show";

            var user = userService.GetCurrentUser(this.User);

            var task = projectService.GetTask(user, id);
            if (task == null)
                return RedirectToAction("Index");

            var model = new TaskShowViewModel() { Task = task };
            return View(model);
        }
    }
}
