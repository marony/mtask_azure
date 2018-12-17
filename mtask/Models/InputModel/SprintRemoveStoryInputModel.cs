using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Models.DomainModel;

namespace mtask.Models.InputModel
{
    public class SprintRemoveStoryInputModel
    {
        public Project Project { get; set; }
        public Sprint Sprint { get; set; }

        public string SprintId { get; set; }
        public string StoryId { get; set; }
        public string StoryDescription { get; set; }
    }
}