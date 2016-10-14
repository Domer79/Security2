using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Configurations;
using Security.Interfaces.Tests;
using Security.Model;
using Security.Model.Entities;
using Tools.Extensions;

namespace Security.EntityFramework.Tests
{
    [TestClass]
    public class SecurityWorkUnitTest : ISecurityWorkUnitTest
    {
        private BaseSecurity _security;
        private readonly SecurityContext _context = new SecurityContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SecurityWorkUnitTest()
        {
            SecurityInit();
        }

        /// <summary>
        /// Первичная настройка параметров. Настройка интерфейсов, первичная установка типов доступа
        /// </summary>
        [TestMethod]
        public void SecurityInit()
        {
            Config.RegisterCommonModule<CommonModule>();
            Config.RegisterAccessTypes(typeof(EAccessType));
            _security = new BaseSecurity();
        }

        /// <summary>
        /// Тест. Добавление пользователя
        /// </summary>
        [TestMethod]
        public void AddUserTest()
        {
            _security.UserCollection.Add(new User() { Login = "User1", Password = "password".GetHashBytes() });
            _security.UserCollection.SaveChanges();
            var user1 = _security.UserCollection.First(e => e.Login == "User1");

            Assert.IsNotNull(user1);
        }

        /// <summary>
        /// Тест. Добавление роли
        /// </summary>
        [TestMethod]
        public void AddRoleTest()
        {
            _security.RoleCollection.Add(new Role() { Name = "Role1" });
            _security.RoleCollection.SaveChanges();
            var role = _security.RoleCollection.First(e => e.Name == "Role1");

            Assert.AreEqual("Role1", role.Name);
        }

        /// <summary>
        /// Тест. Добавление объекта безопасности
        /// </summary>
        [TestMethod]
        public void AddSecObjectTest()
        {
            _security.SecObjectCollection.Add(new SecObject() { ObjectName = "SecObject1" });
            _security.SecObjectCollection.SaveChanges();
            var secObject = _security.SecObjectCollection.First(e => e.ObjectName == "SecObject1");

            Assert.AreEqual("SecObject1", secObject.ObjectName);
        }

        /// <summary>
        /// Тест. Добавление разрешение на операцию Select для объекта на определенную роль
        /// </summary>
        [TestMethod]
        public void AddGrantSelectForSecObject1ToRole1()
        {
            _security.AddGrant("Role1", "SecObject1", EAccessType.Select);

            Assert.AreEqual("Role1", _context.Grants.Include("Role").First().Role.Name);
            Assert.AreEqual("SecObject1", _context.Grants.Include("SecObject").First().SecObject.ObjectName);
            Assert.AreEqual("Select", _context.Grants.Include("AccessType").First().AccessType.Name);
        }

        /// <summary>
        /// Тест. Предоставление роли пользователю
        /// </summary>
        [TestMethod]
        public void AddRole1ToUser1Test()
        {
            _security.AddRole("Role1", "User1");

            Assert.AreEqual("Role1", _context.Roles.First().Name);
            Assert.AreEqual("User1", _context.Members.First().Name);
        }

        /// <summary>
        /// Тест. Проверка входа
        /// </summary>
        [TestMethod]
        public void LogInTest()
        {
            Assert.IsTrue(_security.LogIn("User1", "password"));
        }

        /// <summary>
        /// Тест. Проверка входа с неверным паролем
        /// </summary>
        [TestMethod]
        public void LogInFailedTest()
        {
            Assert.IsTrue(!_security.LogIn("User1", "wrongpassword"));
        }

        /// <summary>
        /// Тест. Проверка доступа
        /// </summary>
        [TestMethod]
        public void CheckAccessTest()
        {
            Assert.IsTrue(_security.CheckAccess("User1", "SecObject1", EAccessType.Select));
        }

        /// <summary>
        /// Тест. Проверка запрещенного доступа
        /// </summary>
        [TestMethod]
        public void CheckAccessWrongTest()
        {
            Assert.IsTrue(!_security.CheckAccess("User1", "SecObject1", EAccessType.Add));
        }

        [TestMethod]
        public void RemoveGrantTest()
        {
            _security.RemoveGrant("Role1", "SecObject1", EAccessType.Select);

            Assert.AreEqual(0, _context.Grants.Count());
        }
    }
}
