using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Newtonsoft.Json;
using NLog;
using Security.Manager.Controllers;
using Security.Manager.Infrastructure;

namespace Security.Manager
{
    public class SecurityControllerFactory : DefaultControllerFactory
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            _logger.Debug($"Request: {requestContext.HttpContext.Request.Path}");
            var appName = (string)requestContext.RouteData.Values["appname"];
            _logger.Trace($"ApplicationName: {appName}");

            if (appName != null)
            {
                _logger.Trace($"Create SecurityController with {appName}");
                return new SecurityController {ApplicationName = appName};
            }

            _logger.Trace($"Create base controller with {requestContext.RouteData.DataTokens.Aggregate("", (c, n) => c + $"key: {n.Key}, value: {n.Value};")}; controllerName: {controllerName}");
            return base.CreateController(requestContext, controllerName);
        }
    }

    public abstract class BaseSecurityController : Controller
    {
        private CoreSecurity _coreSecurity;

        internal string ApplicationName { get; set; }

        public CoreSecurity CoreSecurity
        {
            get { return _coreSecurity ?? (_coreSecurity = new CoreSecurity(ApplicationName)); }
        }

        public ActionResult JsonByNewtonsoft(object data)
        {
            return new NewtonsoftJsonResult(data, JsonRequestBehavior.DenyGet, TypeNameHandling.Objects, PreserveReferencesHandling.Objects);
        }
        
        public ActionResult JsonByNewtonsoft(object data, JsonRequestBehavior behavior)
        {
            return new NewtonsoftJsonResult(data, behavior, TypeNameHandling.Objects, PreserveReferencesHandling.Objects);
        }
        
        protected override void Dispose(bool disposing)
        {
            CoreSecurity.Dispose();
            base.Dispose(disposing);
        }
    }
}