using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace mtask.Models.DomainModel
{
    [Serializable]
    public class Sprint
    {
        public Sprint()
        {
            this.Status = Status.Wait;
            this.RecordStatus = RecordStatus.Active;

            this.InsertedAt = this.UpdatedAt = DateTime.Now;
        }

        public Sprint(string userId, string description)
        {
            this.UserId = userId;
            this.Id = Guid.NewGuid().ToString();
            this.Description = description;
            this.Status = Status.Wait;
            this.RecordStatus = RecordStatus.Active;

            this.InsertedAt = this.UpdatedAt = DateTime.Now;
        }

        public string SerializeVersion { get; set; } = "v1.0";
        public string UserId { get; set; }
        public string Id { get; set; }

        public string Description { get; set; }
        public Status Status { get; set; }

        public List<Story> Stories { get; set; } = new List<Story>();

        public RecordStatus RecordStatus { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public Project Project { get; set; }

        [JsonIgnore]
        public Tuple<int, int> Point
        {
            get
            {
                return Stories.Aggregate(Tuple.Create(0, 0),
                    (acc, st) => Tuple.Create(acc.Item1 + st.Point.Item1, acc.Item2 + st.Point.Item2));
            }
        }

        public void DeserializedSetup(Project project)
        {
            this.Project = project;
            foreach (var story in Stories)
                story.DeserializedSetup(this);
        }

        public Task FindTask(string id)
        {
            foreach (var story in Stories)
            {
                var task = story.GetTask(id);
                if (task != null)
                    return task;
            }
            return null;
        }

        public Story GetStory(string id)
        {
            return Stories.FirstOrDefault(st => st.Id == id);
        }

        public void AddStory(Story story)
        {
            story.Project = null;
            story.Sprint = this;
            Stories.Add(story);
        }

        public bool RemoveStory(Story story)
        {
            story.Project = null;
            story.Sprint = null;
            return Stories.Remove(story);
        }
    }
}