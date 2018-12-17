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
    public class TaskAddInputModel
    {
        public Story Story { get; set; }
        public string StoryId { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("説明")]
        public string Description { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        [DisplayName("ポイント")]
        public int Point { get; set; }
    }
}
