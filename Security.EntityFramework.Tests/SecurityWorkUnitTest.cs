using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using Ninject.Modules;
using Ninject.Web.Common;
using NUnit.Framework;
using Security.Configurations;
using Security.EntityDal;
using Security.Interfaces;
using Security.Interfaces.Tests;
using Security.Model;
using Group = Security.Model.Group;
using Resources = Security.EntityFramework.Tests.Properties.Resources;
using User = Security.Model.User;

namespace Security.EntityFramework.Tests
{
    public class DiModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISecurity>().To<CoreSecurity>().InRequestScope();
        }
    }

    [TestFixture]
    public class SecurityWorkUnitTest : ISecurityWorkUnitTest
    {
        private readonly SecurityContext _context = new SecurityContext();

        [OneTimeSetUp]
        public void Initialize()
        {
            PrepareDatabaseTest();
            SecurityInit();
//            _scope = new TransactionScope();
//            context.Properties.
//            _scope = new TransactionScope();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
//            _scope.Dispose();
            DeleteData();
        }

        [SetUp]
        public void RunBeforeAnyTests()
        {
            InsertData();
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            DeleteData();
        }

        /// <summary>
        /// Первичная настройка параметров. Настройка интерфейсов, первичная установка типов доступа
        /// </summary>
        public static void SecurityInit()
        {
            Config.RegisterCommonModule<CommonModule>();
            Config.RegisterCommonModule<DiModule>();
        }

        [Test]
        public void AddFakeData()
        {
            using (var security = new CoreSecurity())
            {
                const string application1 = "Application1";

                security.ApplicationCollection.Add(new Application() {AppName = application1});
                security.SaveChanges();

                Assert.AreEqual(application1,
                    security.ApplicationCollection.First(e => e.AppName == application1).AppName);
            }
        }

        /// <summary>
        /// Первичная настройка параметров. Настройка интерфейсов, первичная установка типов доступа
        /// </summary>
        void ISecurityWorkUnitTest.SecurityInit()
        {
            SecurityInit();
        }

        /// <summary>
        /// Тест. Добавление пользователя
        /// </summary>
        [Test]
        public void AddUserTest()
        {
            using(var security = new CoreSecurity())
            {
                security.UserCollection.Add(new User()
                {
                    Login = "User1",
                    FirstName = "User1",
                    LastName = "User",
                    Email = "user1@mail.ru"
                });

                security.SaveChanges();
                security.Tools.SetPassword("User1", "password");
                var user1 = security.UserCollection.First(e => e.Login == "User1");

                Assert.IsNotNull(user1);
                
            }
        }

        /// <summary>
        /// Тест. Добавление роли
        /// </summary>
        [Test]
        public void AddRoleTest()
        {
            using (var security = new CoreSecurity())
            {
                security.RoleCollection.Add(new Role()
                {
                    Name = "Role1",
                    Application = (Application) security.CurrentApplication
                });
                security.RoleCollection.SaveChanges();
                var role = security.RoleCollection.First(e => e.Name == "Role1");

                Assert.AreEqual("Role1", role.Name);
            }
        }

        /// <summary>
        /// Тест. Добавление объекта безопасности
        /// </summary>
        [Test]
        public void AddSecObjectTest()
        {
            using (var security = new CoreSecurity())
            {
                security.SecObjectCollection.Add(new SecObject()
                {
                    ObjectName = "SecObject1",
                    Application = (Application) security.CurrentApplication
                });
                security.SecObjectCollection.SaveChanges();
                var secObject = security.SecObjectCollection.First(e => e.ObjectName == "SecObject1");

                Assert.AreEqual("SecObject1", secObject.ObjectName);
            }
        }

        /// <summary>
        /// Тест. Добавление разрешение на операцию Select для объекта на определенную роль
        /// </summary>
        [Test]
        public void AddGrant_CheckGrantAfterInsert_ExpectedRole1SecObject1Exec()
        {
            using (var security = new CoreSecurity())
            {
                security.RoleCollection.Add(new Role() {Name = "Role1"});
                security.SecObjectCollection.Add(new SecObject() {ObjectName = "SecObject1"});
                security.SaveChanges();

                var grant = security.GrantCollection.Add("Role1", "SecObject1", EAccessType.Exec, "Default");
                security.GrantCollection.SaveChanges();

                Assert.AreEqual("Role1", security.GrantCollection.Include("Role").First().Role.Name);
                Assert.AreEqual("SecObject1", security.GrantCollection.Include("SecObject").First().SecObject.ObjectName);
                Assert.AreEqual("Exec", security.GrantCollection.Include("AccessType").First().AccessType.Name);
            }
        }

        /// <summary>
        /// Тест. Предоставление роли пользователю
        /// </summary>
        [Test]
        [TestCase("User1", new[] {"role1", "role2"}, "Default")]
        [TestCase("User2", new[] { "role1", "role2" }, "Default")]
        public void AddRolesToMember_CheckRelationShip_ExpectedValue(string user, string[] roles, string app)
        {
            using (var security = new CoreSecurity(app))
            {
                security.UserCollection.Add(new User()
                {
                    Login = user,
                    FirstName = user,
                    LastName = "User",
                    Email = "user1@mail.ru"
                });

                foreach (var role in roles)
                {
                    security.RoleCollection.Add(new Role() {Name = role});
                }

                security.SaveChanges();

                security.Tools.AddRolesToMember(user, roles, app);

                var member = _context.Members.Include("Roles").First(e => e.Name.Equals(user, StringComparison.OrdinalIgnoreCase));
                Assert.AreEqual(user, member.Name);
                foreach (var role in member.Roles)
                {
                    Assert.IsTrue(roles.Contains(role.Name));
                }
            }
        }

        /// <summary>
        /// Тест. Проверка входа
        /// </summary>
        [Test]
        [TestCase("user1", "password")]
        public void LogIn_CheckAuthentication_ExpectedTrue(string user, string password)
        {
            using (var security = new CoreSecurity())
            {
                security.UserCollection.Add(new User() {Login = user, FirstName = user, LastName = "user", Email = "user@mail.ru"});
                security.SaveChanges();
                security.Tools.SetPassword(user, password);

                var logIn = security.LogIn(user, password);

                Assert.IsTrue(logIn);
            }
        }

        /// <summary>
        /// Тест. Проверка входа с неверным паролем
        /// </summary>
        [Test]
        [TestCase("user1", "password")]
        public void LogIn_CheckAuthentication_ExpectedFalse(string user, string password)
        {
            using (var security = new CoreSecurity())
            {
                security.UserCollection.Add(new User() { Login = user, FirstName = user, LastName = "user", Email = "user@mail.ru" });
                security.SaveChanges();
                security.Tools.SetPassword(user, password);

                var logIn = security.LogIn(user, "wrongpassword");

                Assert.IsTrue(!logIn);
            }
        }

        /// <summary>
        /// Тест. Проверка доступа
        /// </summary>
        [Test]
        public void CheckAccessTest()
        {
            using (var security = new CoreSecurity())
            {
                using (var transaction = security.BeginTransaction())
                {
                    try
                    {
                        if (!security.UserCollection.Any(e => e.Login == "User1"))
                        {
                            security.UserCollection.Add(new User()
                            {
                                Login = "User1",
                                LastName = "User",
                                FirstName = "User1",
                                Email = "User1@mail.ru",
                                Status = true
                            });
                        }
                        else
                        {
                            var user = security.UserCollection.First(e => e.Login == "User1");
                            user.Status = true;
                            security.UserCollection.Update(user);
                        }

                        security.RoleCollection.Add(new Role()
                        {
                            Name = "RoleForCheck",
                            Application = (Application) security.CurrentApplication
                        });
                        security.SecObjectCollection.Add(new SecObject()
                        {
                            ObjectName = "SecObjectForCheck",
                            Application = (Application) security.CurrentApplication
                        });
                        security.SaveChanges();
                        security.GrantCollection.Add("RoleForCheck", "SecObjectForCheck", EAccessType.Exec, "Default");
                        security.Tools.AddRolesToMember("User1", new[] {"RoleForCheck"},
                            security.CurrentApplication.AppName);
                        security.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                Assert.IsTrue(security.CheckAccess("User1", "SecObjectForCheck", EAccessType.Exec));
            }
        }

        /// <summary>
        /// Тест. Проверка запрещенного доступа
        /// </summary>
        [Test]
        public void CheckAccessWrongTest()
        {
            using (var security = new CoreSecurity())
            {
                Assert.IsTrue(!security.CheckAccess("User1", "SecObject1", EAccessType.Exec));
            }
        }

        [Test]
        [TestCase("Group1", new[] {"user1", "user2"})]
        public void AddUsersToGroup_CheckRelationship(string group, string[] users)
        {
            var security = Config.Get<ISecurity>();
            security.GroupCollection.Add(new Group());
            foreach (var user in users)
            {
                security.UserCollection.Add(new User() {Login = user, FirstName = user, LastName = "user", Email = $"{user}@mail.ru", Status = true });
            }

            security.Tools.AddUsersToGroup(group, users);
        }

        [Test]
        public void AddGroupsToUserTest()
        {
            using (var security = new CoreSecurity())
            {
                security.Tools.AddGroupsToUser("user1", new[] {"Группа5", "Группа7"});
            }
        }

        [Test]
        public void DeleteGroupsFromUserTest()
        {
            using (var security = new CoreSecurity())
            {
                security.Tools.DeleteGroupsFromUser("user1", new[] {"Group3", "Group4"});
            }
        }

        [Test]
        public void DeleteUsersFromGroupTest()
        {
            using (var security = new CoreSecurity())
            {
                security.Tools.DeleteUsersFromGroup("Group1", new[] {"User1", "User2"});
            }
        }

        [Test]
        public void AddGroupsToUserByBaseSecurity()
        {
            using (var security = new CoreSecurity())
            {
                var user = security.UserCollection.Include("Groups").First(e => e.Login == "User1");
                var @group = security.GroupCollection.First(e => e.Name == "Группа5");

                user.Groups.Add(group);

                security.SaveChanges();
            }
        }

        #region Helpers

        internal static void PrepareDatabaseTest()
        {
            CreateDatabaseIfNotExists();
        }

        internal static void CreateDatabaseIfNotExists()
        {
            if (ExistDatabase())
                return;

            CreateDatabase();
        }

        internal static void ExecuteQuery(string query, string connectionString = null)
        {
            connectionString = connectionString ?? ConfigurationManager.ConnectionStrings["SecurityDevConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = query;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        internal static void CreateDatabase()
        {
            var sqlCommands = Regex.Split(Resources.DatabaseInited, "\r\ngo", RegexOptions.IgnoreCase);
            var connectionString = ConfigurationManager.ConnectionStrings["SecurityDevConnection"].ConnectionString;
            string originalDbName;
            var masterConnectionString = GetMasterConnectionString(connectionString, out originalDbName);
            ExecuteQuery($"Create database {originalDbName}", masterConnectionString);

            foreach (var command in sqlCommands.Where(command => !string.IsNullOrEmpty(command)))
            {
                string dbname;
                if (CommandIsUse(command, out dbname))
                    continue;

                ExecuteQuery(command, connectionString);
            }
        }

        internal static bool CommandIsUse(string command, out string dbname)
        {
            var match = Regex.Match(command, @"^use \[{0,1}(?<dbname>[\w]+)\]{0,1}$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                dbname = match.Groups["dbname"].Value;
                return true;
            }

            dbname = null;
            return false;
        }

        internal static void InsertData()
        {
            ExecuteQuery(Resources.DataScript);
        }

        internal static void DeleteData()
        {
            ExecuteQuery(Resources.DataDeleteScript);
        }

        internal static bool ExistDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SecurityDevConnection"].ConnectionString;
            string originalDbName;
            connectionString = GetMasterConnectionString(connectionString, out originalDbName);

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.CommandText = $"select DB_ID(N'{originalDbName}') as dbid";
                    connection.Open();
                    var reader = command.ExecuteReader(CommandBehavior.SingleRow);

                    reader.Read();
                    var result = reader["dbid"];

                    return result != DBNull.Value;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Возвращает строку подключения, где база заменена на master
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="originalDbName"></param>
        /// <returns></returns>
        internal static string GetMasterConnectionString(string connectionString, out string originalDbName)
        {
            var sb = new SqlConnectionStringBuilder(connectionString);
            originalDbName = sb.InitialCatalog;
            sb.InitialCatalog = "master";
            return sb.ToString();
        }

        #endregion
    }
}
