using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mtask.Controllers;

namespace mtask.Lib
{
    public static class ControllerUtil
    {
        public static void MergeTempData(Controller controller)
        {
            var modelState = controller.TempData["ModelState"] as ModelStateDictionary;
            if (modelState != null)
                controller.ModelState.Merge(modelState);
        }

        public static void AddToTempData(Controller controller)
        {
            controller.TempData["ModelState"] = controller.ModelState;
        }
    }
}