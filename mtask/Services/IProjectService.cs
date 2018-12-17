using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Models.DomainModel;

namespace mtask.Services
{
    public interface IProjectService : IService
    {
        Project GetProject(User user, string id);
        Project AddProject(ModelStateDictionary modelState, User user, string id, string description);
        Project EditProject(ModelStateDictionary modelState, User user, string id, string description);
        Project DeleteProject(ModelStateDictionary modelState, User user, string id);
        List<Project> GetAllProjects(User user);

        Sprint GetSprint(User user, string id);
        Sprint AddSprint(ModelStateDictionary modelState, User user, string projectId, string description);
        Sprint EditSprint(ModelStateDictionary modelState, User user, string id, string description);
        Sprint DeleteSprint(ModelStateDictionary modelState, User user, string id);
        List<Sprint> GetAllSprints(User user, string projectId);

        Story AddStoryToSprint(ModelStateDictionary modelState, User user, string sprintId, string id);
        Story RemoveStoryFromSprint(ModelStateDictionary modelState, User user, string sprintId, string id);

        Story GetStory(User user, string id);
        Story AddStory(ModelStateDictionary modelState, User user, string projectId, string description);
        Story EditStory(ModelStateDictionary modelState, User user, string id, string description);
        Story DeleteStory(ModelStateDictionary modelState, User user, string id);
        List<Story> GetAllStories(User user, string projectId);

        Task GetTask(User user, string id);
        Task AddTask(ModelStateDictionary modelState, User user, string storyId, string description, int point);
        Task EditTask(ModelStateDictionary modelState, User user, string id, string description, int point, Status status);
        Task DeleteTask(ModelStateDictionary modelState, User user, string id);
        List<Task>  GetAllTasks(User user, string projectId);
    }
}