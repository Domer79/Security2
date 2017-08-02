using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Security.Manager.Models.Security;
using Security.Manager.Attributes;
using Security.EntityFramework.Infrastructure;
using Security.Model;
using System.Net;

namespace Security.Manager.Controllers
{
    public class SettingsController : BaseSecurityController
    {        
        public ActionResult GetSystemSettings()
        {
            var settings = CoreSecurity.Settings.GetSystemSettings();
            return JsonByNewtonsoft(settings, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSetting(Setting setting)
        {
            CoreSecurity.Settings.SetValue<string>(setting.Name, setting.Value);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult GetMainApplicationHostAndPort()
        {
            var host = CoreSecurity.Settings[SystemSettings.MainApplicationHostName.ToString()];
            var port= CoreSecurity.Settings[SystemSettings.MainApplicationPort.ToString()];

            return JsonByNewtonsoft(new { Host = host, Port = port }, JsonRequestBehavior.AllowGet);
        }
    }
}