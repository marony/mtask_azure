using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;

namespace mtask
{
    /// <summary>
    /// 設定系ヘルパ
    /// </summary>
    public static class ConfigulationHelpers
    {
        /// <summary>
        /// 設定をMvcHtmlStringとして取得
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="configulationName"></param>
        /// <returns></returns>
        public static MvcHtmlString Configulation(this HtmlHelper helper, string configulationName)
        {
            return MvcHtmlString.Create(CloudConfigurationManager.GetSetting(configulationName));
        }
    }
}