using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Extensions;

namespace Security.Model.Tests.ToolsTest
{
    [TestClass]
    public class ConfigHelperTests
    {
        [TestMethod]
        public void GetAppSettingsTest()
        {
            var connectionStringName = ConfigHelper.GetAppSettings<string>("SecurityConnectionName");
            Assert.AreEqual("SecurityDevConnection", connectionStringName);
        }
    }
}
