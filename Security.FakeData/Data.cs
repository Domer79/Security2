using System.Collections.Generic;
using Security.FakeData.Infrastructure;
using Security.FakeData.Model;
using Security.Interfaces.Model;

namespace Security.FakeData
{
    public class Data
    {
        public static List<IUser> UserCollection { get; set; } = new List<IUser>();

        public static List<IGroup> GroupCollection { get; set; } = new List<IGroup>();

        public static List<IRole> RoleCollection { get; set; } = new List<IRole>();

        public static List<ISecObject> SecObjectCollection { get; set; } = new List<ISecObject>();

        public static List<IGrant> GrantCollection { get; set; } = new List<IGrant>();

        public static List<IAccessType> AccessTypeCollection { get; set; } = new List<IAccessType>();

        public static List<IMember> MemberCollection { get; set; } = new List<IMember>();

        public static LinkDictionary<Member, Role> MemberRoles { get; set; } = new LinkDictionary<Member, Role>();

        public static LinkDictionary<User, Group> UserGroups { get; set; } = new LinkDictionary<User, Group>();
    }
}