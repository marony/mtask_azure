using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mtask.Models.DomainModel
{
    public enum Status : byte
    {
        [Display(Name = "待機")]
        Wait = 1,
        [Display(Name = "作業中")]
        Running,
        [Display(Name = "完了")]
        Finish,
        [Display(Name = "未アーカイブ")]
        NotArchived,
        [Display(Name = "拒否")]
        Rejected,    
    }
}