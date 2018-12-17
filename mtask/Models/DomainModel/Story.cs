using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace mtask.Models.DomainModel
{
    [Serializable]
    public class Story
    {
        public Story()
        {
            this.Status = Status.Wait;
            this.RecordStatus = RecordStatus.Active;

            this.InsertedAt = this.UpdatedAt = DateTime.Now;
        }

        public Story(string userId, string description)
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

        public List<Task> Tasks { get; set; } = new List<Task>();

        public RecordStatus RecordStatus { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public Project Project { get; set; }
        [JsonIgnore]
        public Sprint Sprint { get; set; }

        [JsonIgnore]
        public Tuple<int, int> Point
        {
            get
            {
                return Tasks.Aggregate(Tuple.Create(0, 0), (acc, t) =>
                    t.Status == Status.Wait || t.Status == Status.Running
                        ? Tuple.Create(acc.Item1, acc.Item2 + t.Point)
                        : Tuple.Create(acc.Item1 + t.Point, acc.Item2 + t.Point));
            }
        }

        public void DeserializedSetup(Project project)
        {
            this.Project = project;
            this.Sprint = null;
            foreach (var task in Tasks)
                task.DeserializedSetup(this);
        }

        public void DeserializedSetup(Sprint sprint)
        {
            this.Project = null;
            this.Sprint = sprint;
            foreach (var task in Tasks)
                task.DeserializedSetup(this);
        }

        public Task GetTask(string id)
        {
            return Tasks.FirstOrDefault(st => st.Id == id);
        }

        public void AddTask(Task task)
        {
            task.Story = this;
            Tasks.Add(task);
        }

        public bool RemoveTask(Task task)
        {
            task.Story = null;
            return Tasks.Remove(task);
        }
    }
}