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

        //todo: Начать отсюда
        //Ознакомиться с https://msdn.microsoft.com/en-us/data/jj591617.aspx
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
                Assert.IsTrue(context.Users.Any(e => e.Login == member + 1));
            }
        }
    }
}
