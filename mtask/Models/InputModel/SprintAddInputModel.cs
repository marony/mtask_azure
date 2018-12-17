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
    public class SprintAddInputModel
    {
        public string ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("説明")]
        public string Description { get; set; }
    }
}
