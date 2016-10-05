using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Model.Entities;
using Tools.Extensions;

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
                var password = "password".GetHashBytes();
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
                        Login = userName,
                        Password = password
                    };

                    context.Users.Add(user);
                    break;
                }

                context.SaveChanges();
                Assert.IsTrue(context.Users.Any(e => e.Login == member + i));
            }
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
        public void CreateAndGetAccessTypeTest()
        {
            
        }
    }
}
