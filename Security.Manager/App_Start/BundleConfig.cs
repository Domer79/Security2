using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Security.Manager.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/angular-ui-router").Include(
                "~/scripts/angular-ui-router.1.0.0.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/angular-material-package").Include(
                "~/scripts/angular/angular.js",
                "~/scripts/angular-animate/angular-animate.js",
                "~/scripts/angular-aria/angular-aria.js",
                "~/scripts/angular-messages.js",
                "~/scripts/angular-material/angular-material.js",
                "~/scripts/angular-sanitize.js"
                ));

            bundles.Add(new ScriptBundle("~/scripts/security").IncludeDirectory("~/scripts/angapp/security", "*.js", true));

            bundles.Add(new StyleBundle("~/styles/angular-material").Include(
                "~/content/angular-material.css"
                ));
            bundles.Add(new StyleBundle("~/styles/bootstrap").Include(
              "~/content/bootstrap.css"
              ));
            bundles.Add(new StyleBundle("~/styles/pss_styles").Include(
               "~/content/reset.css",
               "~/content/common-layout.css"
               ));
        }
    }
}