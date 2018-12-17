using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.ViewModel
{
    public class StoryIndexViewModel
    {
        public Project Project { get; set; }
        public List<Story> Stories { get; set; }
    }
}