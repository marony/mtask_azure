using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.ViewModel
{
    public class StoryShowViewModel
    {
        public Project Project { get; set; }
        public Sprint Sprint { get; set; }
        public Story Story { get; set; }
    }
}