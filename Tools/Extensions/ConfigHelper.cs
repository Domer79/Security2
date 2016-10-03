using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public  static class ConfigHelper
    {
        public static T GetAppSettings<T>(string parameterName)
        {
            var appSetting = ConfigurationManager.AppSettings[parameterName];
            return appSetting != null ? (T)Convert.ChangeType(appSetting, typeof(T)) : default(T);
        }
    }
}
