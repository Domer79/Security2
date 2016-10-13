using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security.Configurations;
using Security.Interfaces.Collections;
using Security.Interfaces.Model;
using Security.Tests.Collections;
using Security.Tests.Common;
using Security.Tests.Model;
using Tools.Extensions;

namespace Security.Tests.Tests
{
    [TestClass]
    public class SecurityWorkUnitTest
    {
        private readonly BaseSecurity _security;

        public SecurityWorkUnitTest()
        {
            Config.RegisterCommonModule<CommonModule>();
            Config.RegisterAccessTypes(typeof(EAccessType));

            _security = new BaseSecurity();
        }

        [TestMethod]
        public void WorkTest()
        {
            _security.UserCollection.Add(new User() {Login = "user1", Password = "password".GetHashBytes()});
            var user1 = _security.UserCollection.First(e => e.Login == "user1");

            Assert.IsNotNull(user1);
        }
    }

    public class Data
    {
        public static List<IUser> UserCollection { get; set; } = new List<IUser>();

        public static List<IGroup> GroupCollection { get; set; } = new List<IGroup>();

        public static List<IRole> RoleCollection { get; set; } = new List<IRole>();

        public static List<ISecObject> SecObjectCollection { get; set; } = new List<ISecObject>();

        public static List<IGrant> GrantCollection { get; set; } = new List<IGrant>();

        public static List<IAccessType> AccessTypeCollection { get; set; } = new List<IAccessType>();

        public static List<IMember> MemberCollection { get; set; } = new List<IMember>();

        public static LinkDictionary<Member, Role> MemberRoles { get; set; }

        public static LinkDictionary<User, Group> UserGroups { get; set; }
    }
}