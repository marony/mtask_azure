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
    public class SprintAddStoryInputModel
    {
        public Project Project { get; set; }
        public string SprintId { get; set; }

        [Required]
        public string SelectedStoryId { get; set; }
        public List<SelectListItem> Stories { get; set; } = new List<SelectListItem>();
    }
}