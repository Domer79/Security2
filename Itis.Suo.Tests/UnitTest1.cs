using System;
using System.Diagnostics;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Manager;
using Security.Manager.Controllers;
using Security.Web;

namespace Itis.Suo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Config.RegisterSecurityAssembly(typeof(AuthController).Assembly);
            var securityObjects = Config.GetSecurityObjects();

            foreach (var securityObject in securityObjects)
            {
                Debug.WriteLine(securityObject.ObjectName);
            }
        }
    }
}
