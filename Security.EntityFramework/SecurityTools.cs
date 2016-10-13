using System;
using System.Linq;
using Security.Exceptions;
using Security.Interfaces;
using Security.Model;

namespace Security.EntityFramework
{
    public class SecurityTools : ISecurityTools, IDisposable
    {
        private readonly SecurityContext _context = new SecurityContext();

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="member">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        public bool CheckAccess(string member, string secObjectName, string accessType)
        {
//            var res = new SqlParameter()
//            {
//                ParameterName = "@res",
//                SqlDbType = SqlDbType.Int,
//                Direction = ParameterDirection.ReturnValue
//            };

            var result = _context.Database.SqlQuery<int>("select sec.IsAllowByName({0}, {1}, {2})", secObjectName, member, accessType).FirstOrDefault();
            return result == 1;
        }

        /// <summary>
        /// Добавляет роль <see cref="roleName"/> участнику <see cref="memberName"/>
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="memberName"></param>
        public void AddRoleToMember(string roleName, string memberName)
        {
            var member = _context.Members.FirstOrDefault(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            if (member == null)
                throw new MemberMissingException(memberName);

            var role = _context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            if (role == null)
                throw new RoleMissingException(roleName);

            member.Roles.Add(role);
            _context.SaveChanges();
        }

        public void DeleteRoleFromMember(string roleName, string memberName)
        {
            var member = _context.Members.FirstOrDefault(m => m.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));
            if (member == null)
                throw new MemberMissingException(memberName);

            var role = _context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            if (role == null)
                throw new RoleMissingException(roleName);

            member.Roles.Remove(role);
            _context.SaveChanges();
        }

        /// <summary>
        /// Добавляет пользователя в группу <see cref="groupName"/>
        /// </summary>
        /// <param name="login"></param>
        /// <param name="groupName"></param>
        public void AddUserToGroup(string login, string groupName)
        {
            var user = _context.Users.FirstOrDefault(m => m.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new MemberMissingException(login);

            var group = _context.Groups.FirstOrDefault(r => r.Name == groupName);
            if (group == null)
                throw new RoleMissingException(groupName);

            user.Groups.Add(group);
            _context.SaveChanges();
        }

        public void DeleteUserFromGroup(string login, string groupName)
        {
            var user = _context.Users.FirstOrDefault(m => m.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                throw new MemberMissingException(login);

            var group = _context.Groups.FirstOrDefault(r => r.Name == groupName);
            if (group == null)
                throw new RoleMissingException(groupName);

            user.Groups.Remove(group);
            _context.SaveChanges();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}