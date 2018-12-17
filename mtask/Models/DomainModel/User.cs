using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Table;

namespace mtask.Models.DomainModel
{
    [Serializable]
    public class User : IUser<string>
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.RecordStatus = RecordStatus.Active;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // ここにカスタム ユーザー クレームを追加します
            return userIdentity;
        }

        public string SerializeVersion { get; set; } = "v1.0";
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ScreenName { get; set; }

        public string PasswordHash { get; set; }

        public RecordStatus RecordStatus { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Project> Projects { get; set; } = new List<Project>();
        public List<User> History { get; set; } = new List<User>();

        public void DeserializedSetup()
        {
            foreach (var project in Projects)
                project.DeserializedSetup(this);
        }

        public Project FindProject(string id)
        {
            return Projects.FirstOrDefault(p => p.Id == id);
        }

        public Sprint FindSprint(string id)
        {
            foreach (var project in Projects)
            {
                var sprint = project.GetSprint(id);
                if (sprint != null)
                    return sprint;
            }
            return null;
        }

        public Story FindStory(string id)
        {
            foreach (var project in Projects)
            {
                var story = project.FindStory(id);
                if (story != null)
                    return story;
            }
            return null;
        }

        public Task FindTask(string id)
        {
            foreach (var project in Projects)
            {
                var task = project.FindTask(id);
                if (task != null)
                    return task;
            }
            return null;
        }

        public Project GetProject(string id)
        {
            return Projects.FirstOrDefault(p => p.Id == id);
        }

        public void AddProject(Project project)
        {
            project.User = this;
            Projects.Add(project);
        }

        public bool RemoveProject(Project project)
        {
            project.User = null;
            return Projects.Remove(project);
        }
    }
}
