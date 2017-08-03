using System;
using System.Configuration;

namespace Tools.Extensions
{
    public  static class ConfigHelper
    {
        /// <summary>
        /// Получает значение параметра секции appSettings из файла web.config
        /// </summary>
        /// <typeparam name="T">Тип для преобразования полученного значения</typeparam>
        /// <param name="parameterName">Наименование параметра</param>
        /// <returns></returns>
        public static T GetAppSettings<T>(string parameterName)
        {
            var appSetting = ConfigurationManager.AppSettings[parameterName];
            return appSetting != null ? (T)Convert.ChangeType(appSetting, typeof(T)) : default(T);
        }
    }
}
