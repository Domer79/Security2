using System.Linq;
using System.Security.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Configurations;
using Security.FakeData;
using Security.FakeData.Common;
using Security.FakeData.Model;
using Security.Interfaces.Tests;
using Tools.Extensions;

namespace Security.Tests.Tests
{
    [TestClass]
    public class FakeSecurityWorkUnitTest : ISecurityWorkUnitTest
    {
        private BaseSecurity _security;

        [TestMethod]
        public void AllTests()
        {
            SecurityInit();
            AddUserTest();
            AddGroupTest();
            AddRoleTest();
            AddSecObjectTest();
            AddGrantSelectForSecObject1ToRole1();
            AddRole1ToUser1Test();
            LogInTest();
            LogInFailedTest();
            CheckAccessTest();
            CheckAccessWrongTest();
        }

        public void SecurityInit()
        {
            if (Data.AccessTypeCollection.Count == 0)
            {
                Config.RegisterCommonModule<FakeCommonModule>();
                Config.RegisterAccessTypes(typeof (EAccessType));
            }

            _security = new BaseSecurity();
        }

        /// <summary>
        /// Добавление пользователя
        /// </summary>
        public void AddUserTest()
        {
            _security.UserCollection.Add(new User() {Login = "User1", Password = "password".GetHashBytes()});
            _security.UserCollection.SaveChanges();
            var user1 = _security.UserCollection.First(e => e.Login == "User1");

            Assert.IsNotNull(user1);
        }

        public void AddGroupTest()
        {
            _security.GroupCollection.Add(new Group() {Name = "Group1", Description = "Группа1"});
            _security.GroupCollection.SaveChanges();
            var group1 = _security.GroupCollection.First(e => e.Name == "Group1");

            Assert.IsNotNull(group1);
        }

        /// <summary>
        /// Тест. Добавление роли
        /// </summary>
        public void AddRoleTest()
        {
            _security.RoleCollection.Add(new Role() {Name = "Role1"});
            _security.RoleCollection.SaveChanges();
            var role = _security.RoleCollection.First(e => e.Name == "Role1");

            Assert.AreEqual("Role1", role.Name);
        }

        /// <summary>
        /// Тест. Добавление объекта безопасности
        /// </summary>
        public void AddSecObjectTest()
        {
            _security.SecObjectCollection.Add(new SecObject() {ObjectName = "SecObject1"});
            _security.SecObjectCollection.SaveChanges();
            var secObject = _security.SecObjectCollection.First(e => e.ObjectName == "SecObject1");

            Assert.AreEqual("SecObject1", secObject.ObjectName);
        }

        /// <summary>
        /// Тест. Добавление разрешение на операцию Select для объекта на определенную роль
        /// </summary>
        public void AddGrantSelectForSecObject1ToRole1()
        {
            _security.GrantCollection.Add("Role1", "SecObject1", EAccessType.Select);

            Assert.AreEqual("Role1", Data.GrantCollection.First().Role.Name);
            Assert.AreEqual("SecObject1", Data.GrantCollection.First().SecObject.ObjectName);
            Assert.AreEqual("Select", Data.GrantCollection.First().AccessType.Name);
        }

        /// <summary>
        /// Тест. Предоставление роли пользователю
        /// </summary>
        public void AddRole1ToUser1Test()
        {
            _security.AddRole("Role1", "User1");

            Assert.AreEqual("Role1", _security.MemberCollection.First().Roles.First().Name);
            Assert.AreEqual("User1", _security.RoleCollection.First().Members.First().Name);
        }

        /// <summary>
        /// Тест. Проверка входа
        /// </summary>
        public void LogInTest()
        {
            Assert.IsTrue(_security.LogIn("User1", "password"));
        }

        /// <summary>
        /// Тест. Проверка входа с неверным паролем
        /// </summary>
        public void LogInFailedTest()
        {
            Assert.IsTrue(!_security.LogIn("User1", "wrongpassword"));
        }

        /// <summary>
        /// Тест. Проверка доступа
        /// </summary>
        public void CheckAccessTest()
        {
            Assert.IsTrue(_security.CheckAccess("User1", "SecObject1", EAccessType.Select));
        }

        /// <summary>
        /// Тест. Проверка запрещенного доступа
        /// </summary>
        public void CheckAccessWrongTest()
        {
            Assert.IsTrue(!_security.CheckAccess("User1", "SecObject1", EAccessType.Add));
        }
    }
}