using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.ViewModel
{
    public class SprintIndexViewModel
    {
        public Project Project { get; set; }
        public List<Sprint> Sprints { get; set; }
    }
}