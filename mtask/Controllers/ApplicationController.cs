using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mtask.Controllers
{
    /// <summary>
    /// コントローラベースクラス
    /// </summary>
    public class ApplicationController : Controller
    {
        /// <summary>
        /// 例外処理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Console.WriteLine(filterContext.Exception.ToString());
        }
    }
}