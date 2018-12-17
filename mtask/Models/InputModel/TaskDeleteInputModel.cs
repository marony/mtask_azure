using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mtask.Models.DomainModel;

namespace mtask.Models.InputModel
{
    public class TaskDeleteInputModel
    {
        public Story Story { get; set; }
        public string Id { get; set; }

        [DisplayName("ポイント")]
        public int Point { get; set; }
        [DisplayName("説明")]
        public string Description { get; set; }
    }
}