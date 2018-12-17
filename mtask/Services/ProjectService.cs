using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Models;
using mtask.Models.DomainModel;
using Microsoft.Practices.Unity;

namespace mtask.Services
{
    public class ProjectService : IProjectService
    {
        protected readonly IUserService userService = null;

        public ProjectService()
        {
            userService = new UserService();
        }

        [InjectionConstructor]
        public ProjectService(IUserService userService)
        {
            this.userService = userService;
        }

        public Project GetProject(User user, string id)
        {
            if (user == null)
                return null;

            var project = user.FindProject(id);
            return project;
        }

        public Project AddProject(ModelStateDictionary modelState, User user, string id, string description)
        {
            if (user == null)
                return null;

            if (user.FindProject(id) != null)
            {
                modelState.AddModelError("Id", "既にそのIDは存在します。");
                return null;
            }

            var project = new Project(user.Id, id, (description ?? ""));
            project.InsertedAt = project.UpdatedAt = DateTime.Now;
            user.AddProject(project);
            userService.UpdateUser(user);

            return project;
        }

        public Project EditProject(ModelStateDictionary modelState, User user, string id, string description)
        {
            if (user == null)
                return null;

            var project = user.FindProject(id);
            if (project == null)
                return null;

            project.Description = description ?? "";
            project.UpdatedAt = DateTime.Now;
            userService.UpdateUser(user);

            return project;
        }

        public Project DeleteProject(ModelStateDictionary modelState, User user, string id)
        {
            if (user == null)
                return null;

            var project = user.FindProject(id);
            if (project == null)
                return null;

            user.RemoveProject(project);
            userService.UpdateUser(user);

            return project;
        }

        public List<Project> GetAllProjects(User user)
        {
            if (user == null)
                return null;

            return user.Projects;
        }

        public Sprint GetSprint(User user, string id)
        {
            if (user == null)
                return null;

            var sprint = user.FindSprint(id);
            return sprint;
        }

        public Sprint AddSprint(ModelStateDictionary modelState, User user, string projectId, string description)
        {
            if (user == null)
                return null;

            var project = user.FindProject(projectId);
            if (project == null)
                return null;

            var sprint = new Sprint(user.Id, (description ?? ""));
            sprint.InsertedAt = sprint.UpdatedAt = DateTime.Now;
            project.AddSprint(sprint);
            userService.UpdateUser(user);

            return sprint;
        }

        public Sprint EditSprint(ModelStateDictionary modelState, User user, string id, string description)
        {
            if (user == null)
                return null;

            var sprint = user.FindSprint(id);
            sprint.Description = description;
            sprint.UpdatedAt = DateTime.Now;
            userService.UpdateUser(user);

            return sprint;
        }

        public Sprint DeleteSprint(ModelStateDictionary modelState, User user, string id)
        {
            if (user == null)
                return null;

            var sprint = user.FindSprint(id);
            if (sprint == null)
                return null;

            sprint.Project.RemoveSprint(sprint);
            userService.UpdateUser(user);

            return sprint;
        }

        public List<Sprint> GetAllSprints(User user, string projectId)
        {
            if (user == null)
                return null;

            var project = user.GetProject(projectId);
            if (project == null)
                return null;

            return project?.Sprints;
        }

        public Story AddStoryToSprint(ModelStateDictionary modelState, User user, string sprintId, string id)
        {
            if (user == null)
                return null;

            var sprint = user.FindSprint(sprintId);
            if (sprint == null)
                return null;

            var story = user.FindStory(id);
            if (story == null)
                return null;

            var project = sprint.Project;
            // 既にスプリント内に存在したらエラー
            if (project.Sprints.Any(sp => sp.Stories.Exists(st => st.Id == id)))
                return null;

            // プロダクトバックログからの削除に失敗したらエラー
            if (!project.RemoveStory(story))
                return null;

            sprint.AddStory(story);
            project.UpdatedAt = sprint.UpdatedAt = story.UpdatedAt = DateTime.Now;
            userService.UpdateUser(user);

            return story;
        }

        public Story RemoveStoryFromSprint(ModelStateDictionary modelState, User user, string sprintId, string id)
        {
            if (user == null)
                return null;

            var sprint = user.FindSprint(sprintId);
            if (sprint == null)
                return null;

            var story = user.FindStory(id);
            if (story == null)
                return null;

            var project = sprint.Project;
            // プロダクトバックログに存在したらエラー
            if (project.ProductBackLog.Exists(st => st.Id == id))
                return null;

            // このスプリント以外に存在したらエラー
            if (project.Sprints.Any(sp => sp.Id != sprintId && sp.Stories.Exists(st => st.Id == id)))
                return null;

            // スプリントからの削除に失敗したらエラー
            if (!sprint.RemoveStory(story))
                return null;

            project.AddStory(story);
            project.UpdatedAt = sprint.UpdatedAt = story.UpdatedAt = DateTime.Now;
            userService.UpdateUser(user);

            return story;
        }

        public Story GetStory(User user, string id)
        {
            if (user == null)
                return null;

            var story = user.FindStory(id);
            return story;
        }

        public Story AddStory(ModelStateDictionary modelState, User user, string projectId, string description)
        {
            if (user == null)
                return null;

            var project = user.GetProject(projectId);
            if (project == null)
                return null;

            var story = new Story(user.Id, (description ?? ""));
            story.InsertedAt = story.UpdatedAt = DateTime.Now;
            project.AddStory(story);
            userService.UpdateUser(user);

            return story;
        }

        public Story EditStory(ModelStateDictionary modelState, User user, string id, string description)
        {
            if (user == null)
                return null;

            var story = user.FindStory(id);
            if (story == null)
                return null;

            story.Description = description;
            story.UpdatedAt = DateTime.Now;
            userService.UpdateUser(user);

            return story;
        }

        public Story DeleteStory(ModelStateDictionary modelState, User user, string id)
        {
            if (user == null)
                return null;

            var story = user.FindStory(id);
            if (story == null)
                return null;

            if (story.Sprint == null)
            {
                var project = story.Project;
                if (!project.RemoveStory(story))
                    return null;

                project.UpdatedAt = story.UpdatedAt = DateTime.Now;
                userService.UpdateUser(user);

                return story;
            }
            else
            {
                var sprint = story.Sprint;
                if (!sprint.RemoveStory(story))
                    return null;

                sprint.UpdatedAt = story.UpdatedAt = DateTime.Now;
                userService.UpdateUser(user);

                return story;
            }
        }

        public List<Story> GetAllStories(User user, string projectId)
        {
            if (user == null)
                return null;

            var project = user.GetProject(projectId);
            if (project == null)
                return null;

            return project.ProductBackLog;
        }

        public Task GetTask(User user, string id)
        {
            if (user == null)
                return null;

            var task = user.FindTask(id);
            return task;
        }

        public Task AddTask(ModelStateDictionary modelState, User user, string storyId, string description, int point)
        {
            if (user == null)
                return null;

            var story = user.FindStory(storyId);
            if (story == null)
                return null;

            var task = new Task(user.Id, point, (description ?? ""));
            story.UpdatedAt = task.InsertedAt = task.UpdatedAt = DateTime.Now;
            story.AddTask(task);
            userService.UpdateUser(user);

            return task;
        }

        public Task EditTask(ModelStateDictionary modelState, User user, string id, string description, int point, Status status)
        {
            if (user == null)
                return null;

            var task = user.FindTask(id);
            if (task == null)
                return null;

            task.Description = description;
            task.Point = point;
            task.Status = status;
            task.UpdatedAt = DateTime.Now;
            userService.UpdateUser(user);

            return task;
        }

        public Task DeleteTask(ModelStateDictionary modelState, User user, string id)
        {
            if (user == null)
                return null;

            var task = user.FindTask(id);
            if (task == null)
                return null;

            var story = task.Story;

            story.UpdatedAt = task.UpdatedAt = DateTime.Now;
            story.RemoveTask(task);
            userService.UpdateUser(user);

            return task;
        }

        public List<Task> GetAllTasks(User user, string projectId)
        {
            throw new NotImplementedException();
        }
    }
}