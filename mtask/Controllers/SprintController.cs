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
    /// スプリントコントローラ
    /// </summary>
    [Authorize]
    public class SprintController : ApplicationController
    {
        protected readonly IUserService userService = null;
        protected readonly IProjectService projectService = null;

        [InjectionConstructor]
        public SprintController(IUserService userService, IProjectService projectService)
        {
            this.userService = userService;
            this.projectService = projectService;
        }

        /// <summary>
        /// スプリント一覧
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
        /// スプリント追加画面表示
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

            var model = new SprintAddInputModel() { ProjectId = project.Id, Project = project };
            return View(model);
        }

        /// <summary>
        /// スプリント追加処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(SprintAddInputModel model, string returnUrl)
        {
            var action = "Add";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ProjectId = model.ProjectId, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var project = projectService.GetProject(user, model.ProjectId);
            if (project == null)
                return RedirectToAction("Index");
            var sprint = projectService.AddSprint(ModelState, user, project.Id, model.Description);

            if (sprint == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ProjectId = model.ProjectId, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Project", new { Id = project.Id });
            }
        }

        /// <summary>
        /// スプリント編集画面表示
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
            var sprint = projectService.GetSprint(user, id);
            if (sprint == null)
                return RedirectToAction("Index");

            var model = new SprintEditInputModel() { Project = sprint.Project, Id = id, Description = sprint.Description };
            return View(model);
        }

        /// <summary>
        /// スプリント編集処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(SprintEditInputModel model, string returnUrl)
        {
            var action = "Edit";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var sprint = projectService.EditSprint(ModelState, user, model.Id, model.Description);
            if (sprint == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Project", new { Id = sprint.Id });
            }
        }

        /// <summary>
        /// スプリント削除画面表示
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
            var sprint = projectService.GetSprint(user, id);
            if (sprint == null)
                return RedirectToAction("Index");

            var model = new SprintDeleteInputModel() { Project = sprint.Project, Id = id, Description = sprint.Description };
            return View(model);
        }

        /// <summary>
        /// スプリント削除処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(SprintDeleteInputModel model, string returnUrl)
        {
            var action = "Delete";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var sprint = projectService.DeleteSprint(ModelState, user, model.Id);
            if (sprint == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { Id = model.Id, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", "Project", new { Id = sprint.Project.Id });
            }
        }

        /// <summary>
        /// ストーリー追加画面表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddStory(string id, string returnUrl)
        {
            var action = "AddStory";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var user = userService.GetCurrentUser(User);
            var sprint = projectService.GetSprint(user, id);
            if (sprint == null)
                return RedirectToAction("Index");

            var model = new SprintAddStoryInputModel() { Project = sprint.Project, SprintId = id };
            foreach (var st in sprint.Project.ProductBackLog)
                model.Stories.Add(new SelectListItem() { Text = st.Description, Value = st.Id });
            model.SelectedStoryId = model.Stories.SingleOrDefault(st => st.Value == model.SelectedStoryId)?.Value;
            return View(model);
        }

        /// <summary>
        /// ストーリー追加処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddStory(SprintAddStoryInputModel model, string returnUrl)
        {
            var action = "AddStory";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ProjectId = model.SprintId, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var story = projectService.AddStoryToSprint(ModelState, user, model.SprintId, model.SelectedStoryId);
            if (story == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { ProjectId = model.SprintId, ReturnUrl = returnUrl });
            }
            else
            {
                var project = (story.Project ?? story.Sprint.Project);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", new { Id = model.SprintId });
            }
        }

        /// <summary>
        /// ストーリー除外画面表示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RemoveStory(string sprintId, string id, string returnUrl)
        {
            var action = "RemoveStory";

            ControllerUtil.MergeTempData(this);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            var user = userService.GetCurrentUser(User);
            var story = projectService.GetStory(user, id);
            if (story == null)
                return RedirectToAction("Index");

            var project = story.Project ?? story.Sprint.Project;
            var model = new SprintRemoveStoryInputModel() { Project = project, Sprint = story.Sprint, SprintId = sprintId,
                StoryId = id, StoryDescription = story.Description };
            return View(model);
        }

        /// <summary>
        /// ストーリー除外処理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveStory(SprintRemoveStoryInputModel model, string returnUrl)
        {
            var action = "RemoveStory";

            if (!ModelState.IsValid)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { SprintId = model.SprintId, Id = model.StoryId, ReturnUrl = returnUrl });
            }

            var user = userService.GetCurrentUser(User);
            var story = projectService.RemoveStoryFromSprint(ModelState, user, model.SprintId, model.StoryId);
            if (story == null)
            {
                ControllerUtil.AddToTempData(this);
                return RedirectToAction(action, new { SprintId = model.SprintId, Id = model.StoryId, ReturnUrl = returnUrl });
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Show", new { Id = model.SprintId });
            }
        }

        /// <summary>
        /// スプリント(の中身)表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Show(string id)
        {
            var action = "Show";

            var user = userService.GetCurrentUser(this.User);
            var sprint = projectService.GetSprint(user, id);
            if (sprint == null)
                return RedirectToAction("Index");

            var model = new SprintShowViewModel() { Sprint = sprint };
            return View(model);
        }
    }
}
