using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.InputModel
{
    public class TaskEditInputModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("説明")]
        public string Description { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        [DisplayName("ポイント")]
        public int Point { get; set; }
        [Required]
        [DisplayName("状態")]
        public Status Status { get; set; }
    }
}