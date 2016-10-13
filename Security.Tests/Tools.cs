using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Configurations;
using Security.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Tests.Model;
using Security.Tests.Tests;

namespace Security.Tests
{
    public class Tools : ISecurityTools
    {
        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        public bool CheckAccess(string login, string secObjectName, Enum accessType)
        {
            var grantCollection = Config.Get<IGrantCollection>();
            var memberCollection = Config.Get<IMemberCollection>();

            var userRoles = memberCollection.First(e => e.Name == login).Roles;
            var userAccessRoles = grantCollection.Where(e => e.SecObject.ObjectName == secObjectName && e.AccessType.Name == secObjectName).Select(e => e.Role);

            return userRoles.Intersect(userAccessRoles).Any();
        }

        /// <summary>
        /// Добавляет роль <see cref="roleName"/> участнику <see cref="memberName"/>
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="memberName"></param>
        public void AddRoleToMember(string roleName, string memberName)
        {
            var member = (Member)Data.MemberCollection.First(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            if (member == null)
                throw new MemberMissingException(memberName);

            var role = (Role)Data.RoleCollection.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
                throw new RoleMissingException(roleName);

            Data.MemberRoles.Link(member, role);
        }

        public void DeleteRoleFromMember(string roleName, string memberName)
        {
            var member = (Member)Data.MemberCollection.First(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            if (member == null)
                throw new MemberMissingException(memberName);

            var role = (Role)Data.RoleCollection.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
                throw new RoleMissingException(roleName);

            Data.MemberRoles.DeleteLink(member, role);
        }

        /// <summary>
        /// Добавляет пользователя в группу <see cref="groupName"/>
        /// </summary>
        /// <param name="login"></param>
        /// <param name="groupName"></param>
        public void AddUserToGroup(string login, string groupName)
        {
            var user = (User)Data.UserCollection.First(m => m.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new MemberMissingException(login);

            var group = (Group)Data.RoleCollection.FirstOrDefault(r => r.Name == groupName);
            if (group == null)
                throw new RoleMissingException(groupName);

            Data.UserGroups.Link(user, group);
        }

        public void DeleteUserFromGroup(string login, string groupName)
        {
            var user = (User)Data.UserCollection.First(m => m.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new MemberMissingException(login);

            var group = (Group)Data.RoleCollection.FirstOrDefault(r => r.Name == groupName);
            if (group == null)
                throw new RoleMissingException(groupName);

            Data.UserGroups.DeleteLink(user, group);
        }
    }
}
