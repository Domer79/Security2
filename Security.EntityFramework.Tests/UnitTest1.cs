using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Configurations;

namespace Security.EntityFramework.Tests
{
    public enum EAccessType
    {
        Select,
        Add,
        Update,
        Delete
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RegisterAccessTypeTest()
        {
            Config.RegisterCommonModule<CommonModule>();
            Config.RegisterAccessTypes(typeof(EAccessType));
        }
    }
}
