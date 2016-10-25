using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Configurations;
using Security.Exceptions;
using Security.FakeData;
using Security.FakeData.Common;
using Security.FakeData.Model;
using Security.Interfaces.Collections;

namespace Security.Tests.Tests
{
    [TestClass]
    public class AccessTypeUnitTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AccessTypeUnitTest()
        {
            Config.RegisterCommonModule<FakeCommonModule>();
        }

        [TestMethod]
        public void CreateCollectionTest()
        {
            var accessTypeCollection = Config.Get<IAccessTypeCollection>(); 
            Assert.IsNotNull(accessTypeCollection);
        }

        [TestMethod]
        public void AddItemToCollectionTest()
        {
            var accessTypeCollection = Config.Get<IAccessTypeCollection>();
            var accessType = new AccessType {Name = "add"};

            accessTypeCollection.Add(accessType);

            Assert.AreEqual("add", accessTypeCollection.First().Name);
        }

        [TestMethod]
        public void RegisterAccessTypeTest()
        {
            Config.RegisterAccessTypes(typeof(EAccessType));
        }

        [TestMethod]
        [ExpectedException(typeof(CannotModifyAccessTypeException))]
        public void RegisterAccessTypeWithGrantsTest()
        {
            Data.GrantCollection.Add(new Grant());
            Config.RegisterAccessTypes(typeof(EAccessType));
        }
    }

    public enum EAccessType
    {
        Add,
        Update,
        Delete,
        Select
    }
}
