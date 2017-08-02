using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Security.Extensions;

namespace Security.Manager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new {controller = "Main", action = "Index"}
                );

            routes.MapRoute(
                name: "secapp",
                url: "{appname}/{controller}/{action}/{id}",
                defaults: new { appname = typeof(RouteConfig).Assembly.GetSecurityInfoFromAssembly().ApplicationName, controller = "Security", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
