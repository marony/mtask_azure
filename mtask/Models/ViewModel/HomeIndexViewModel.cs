using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Models.DomainModel;

namespace mtask.Models.ViewModel
{
    [Authorize]
    public class HomeIndexViewModel
    {
        public User User { get; set; }
    }
}