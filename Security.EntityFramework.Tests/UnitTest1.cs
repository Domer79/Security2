using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using Security.Configurations;
using Security.EntityDal;
using Security.Model;

namespace Security.EntityFramework.Tests
{
    public enum EAccessType
    {
        Exec
    }

    [TestFixture]
    public class UnitTest1
    {
        private static CoreSecurity _security;

        [OneTimeSetUp]
        public void Initialize()
        {
            SecurityWorkUnitTest.PrepareDatabaseTest();
            SecurityInit();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            SecurityWorkUnitTest.DeleteData();
        }

        /// <summary>
        /// Первичная настройка параметров. Настройка интерфейсов, первичная установка типов доступа
        /// </summary>
        public static void SecurityInit()
        {
            Config.RegisterCommonModule<CommonModule>();

            _security = new CoreSecurity();
        }

        [Test, Category("SecuritySettings")]
        public void SecuritySettingsTest()
        {
            _security.Settings["key1"] = "value1";

            Assert.AreEqual("value1", _security.Settings["key1"]);
        }

        [Test]
        public void SecuritySettingsTest2()
        {
            _security.Settings["key1"] = "value2";

            Assert.AreEqual("value2", _security.Settings["key1"]);
        }

        [Test, Category("SecuritySettings")]
        public void SecuritySettingsTest3()
        {
            _security.Settings.SetValue("key2", 25);

            Assert.AreEqual(25, _security.Settings.GetValue<int>("key2"));
        }
    }
}
