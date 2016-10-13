using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Security.Configurations;
using Security.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Tests.Collections;
using Security.Tests.Common;
using Security.Tests.Model;

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
            Config.RegisterCommonModule<CommonModule>();
        }

        [TestMethod]
        public void CreateCollectionTest()
        {
            var accessTypeCollection = Config.Get<IAccessTypeCollection>(); ;
            Assert.IsNotNull(accessTypeCollection);
        }

        [TestMethod]
        public void AddItemToCollectionTest()
        {
            var accessTypeCollection = Config.Get<IAccessTypeCollection>(); ;
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
