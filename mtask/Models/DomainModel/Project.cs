using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace mtask.Models.DomainModel
{
    [Serializable]
    public class Project
    {
        public Project()
        {
            this.Status = Status.Wait;
            this.RecordStatus = RecordStatus.Active;

            this.InsertedAt = this.UpdatedAt = DateTime.Now;
        }

        public Project(string userId, string id, string description)
        {
            this.UserId = userId;
            this.Id = id;
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

        public List<Sprint> Sprints { get; set; } = new List<Sprint>();
        public List<Story> ProductBackLog { get; set; } = new List<Story>();

        public RecordStatus RecordStatus { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public void DeserializedSetup(User user)
        {
            this.User = user;
            foreach (var sprint in Sprints)
                sprint.DeserializedSetup(this);
            foreach (var story in ProductBackLog)
                story.DeserializedSetup(this);
        }

        [JsonIgnore]
        public Tuple<int, int> Point
        {
            get
            {
                var spPoint = Sprints.Aggregate(Tuple.Create(0, 0),
                    (acc, sp) => Tuple.Create(acc.Item1 + sp.Point.Item1, acc.Item2 + sp.Point.Item2));
                var stPoint = ProductBackLog.Aggregate(Tuple.Create(0, 0),
                    (acc, st) => Tuple.Create(acc.Item1 + st.Point.Item1, acc.Item2 + st.Point.Item2));
                return Tuple.Create(spPoint.Item1 + stPoint.Item1, spPoint.Item2 + stPoint.Item2);
            }
        }

        public Sprint GetSprint(string id)
        {
            return Sprints.FirstOrDefault(sp => sp.Id == id);
        }

        public Story FindStory(string id)
        {
            var story = ProductBackLog.FirstOrDefault(s => s.Id == id);
            if (story != null)
                return story;

            foreach (var sprint in Sprints)
            {
                story = sprint.GetStory(id);
                if (story != null)
                    return story;
            }
            return null;
        }

        public Task FindTask(string id)
        {
            foreach (var story in ProductBackLog)
            {
                var task = story.GetTask(id);
                if (task != null)
                    return task;
            }
            foreach (var sprint in Sprints)
            {
                var task = sprint.FindTask(id);
                if (task != null)
                    return task;
            }
            return null;
        }

        public void AddSprint(Sprint sprint)
        {
            sprint.Project = this;
            Sprints.Add(sprint);
        }

        public bool RemoveSprint(Sprint sprint)
        {
            sprint.Project = null;
            return Sprints.Remove(sprint);
        }

        public Story GetStory(string id)
        {
            return ProductBackLog.FirstOrDefault(st => st.Id == id);
        }

        public void AddStory(Story story)
        {
            story.Project = this;
            story.Sprint = null;
            ProductBackLog.Add(story);
        }

        public bool RemoveStory(Story story)
        {
            story.Project = null;
            story.Sprint = null;
            return ProductBackLog.Remove(story);
        }
    }
}