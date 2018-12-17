using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mtask.Models.InputModel
{
    public class StoryEditInputModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("説明")]
        public string Description { get; set; }
    }
}