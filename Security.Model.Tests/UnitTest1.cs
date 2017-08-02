using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.EntityDal;
using Tools.Extensions;
using System.Diagnostics;

namespace Security.Model.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateContextTest()
        {
            var context = new SecurityContext();
            context.Dispose();
        }

        [TestMethod]
        public void CreateUserTest()
        {
            using (var context = new SecurityContext())
            {
                const string member = "user";
                var password = "password".GetMD5HashBytes();
                var i = 0;

                while (true)
                {
                    var userName = member + i;
                    if (context.Users.Any(e => e.Login == userName))
                    {
                        i++;
                        continue;
                    }

                    var user = new User()
                    {
                        Login = userName
                    };

                    context.Users.Add(user);
                    break;
                }

                context.SaveChanges();
                Assert.IsTrue(context.Users.Any(e => e.Login == member + i));
            }
        }

        [TestMethod]
        public void PasswordHashToString()
        {
            var hashBytes = "password".GetMD5HashBytes();
            var hashString = hashBytes.GetString();
            Debug.WriteLine(hashString);
        }

        [TestMethod]
        public void CreateGroupTest()
        {
            using (var context = new SecurityContext())
            {
                const string member = "group";
                const string description = "description";
                var i = 0;

                while (true)
                {
                    var groupName = member + i;
                    var descriptionName = description + i;

                    if (context.Groups.Any(e => e.Name == groupName))
                    {
                        i++;
                        continue;
                    }

                    var group = new Group()
                    {
                        Name = groupName,
                        Description = descriptionName
                    };

                    context.Groups.Add(group);
                    break;
                }

                context.SaveChanges();
                Assert.IsTrue(context.Groups.Any(e => e.Name == member + i));
            }
        }

        [TestMethod]
        public void GetRolesByUserTest()
        {
            using (var context = new SecurityContext())
            {
                var roles = context.Members.Include("Roles").Where(m => m.Name == "User1").ToList();

                Assert.AreEqual(1, roles.Count());
            }
        }

        [TestMethod]
        public void GetFirstUserTest()
        {
            using (var context = new SecurityContext())
            {
                var user = context.Users.First();
                Assert.IsNotNull(user);
            }
        }

        [TestMethod]
        public void GetGrantListTest()
        {
            using (var context = new SecurityContext())
            {
                var grants = context.Grants.Include("SecObject").Include("Role").Include("AccessType").ToList();
                Assert.AreEqual(1, grants.Count);
            }
        }
    }
}
