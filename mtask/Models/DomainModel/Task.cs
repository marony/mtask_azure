using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace mtask.Models.DomainModel
{
    [Serializable]
    public class Task
    {
        public Task()
        {
            this.Status = Status.Wait;
            this.RecordStatus = RecordStatus.Active;

            this.InsertedAt = this.UpdatedAt = DateTime.Now;
        }

        public Task(string userId, int point, string description)
        {
            this.UserId = userId;
            this.Id = Guid.NewGuid().ToString();
            this.Point = point;
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

        public int Point { get; set; }
        public List<double> WorkTime { get; set; }

        public RecordStatus RecordStatus { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public Story Story { get; set; }

        public void DeserializedSetup(Story story)
        {
            this.Story = story;
        }
    }
}