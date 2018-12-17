using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.InputModel
{
    public class SprintEditInputModel
    {
        public Project Project { get; set; }
        public string Id { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("説明")]
        public string Description { get; set; }
    }
}