using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Security.EntityDal;
using Security.EntityFramework.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Model;
using Security.Model;
using Tools.Extensions;

namespace Security.EntityFramework
{
    /// <summary>
    /// Дополнительный инструментарий для ядра системы доступа
    /// </summary>
    internal class SecurityTools : ISecurityTools
    {
        private readonly SecurityContext _context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст <see cref="SecurityContext"/></param>
        public SecurityTools(SecurityContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="member">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName"></param>
        /// <returns></returns>
        public bool CheckAccess(string member, string secObjectName, string accessType, string appName)
        {
            return _context.Database.SqlQuery<bool>("select sec.IsAllowByName(@p0, @p1, @p2, @p3)", secObjectName, member, accessType, appName).FirstOrDefault();
        }

        /// <summary>
        /// Добавляет роли <see cref="roleNames"/> участнику <see cref="memberName"/>
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="appName"></param>
        /// <param name="roleNames"></param>
        public void AddRolesToMember(string memberName, string[] roleNames, string appName)
        {
            var roles = _context.Roles
                .Include("Application")
                .Where(e => e.Application.AppName.Equals(appName, StringComparison.OrdinalIgnoreCase))
                .Where(e => roleNames.Contains(e.Name));
            var member = _context.Members.First(r => r.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));

            foreach (var role in roles)
            {
                member.Roles.Add(role);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет роли у участника безопасности
        /// </summary>
        /// <param name="memberName">Участник безопасности</param>
        /// <param name="roleNames">Список ролей</param>
        /// <param name="appName">Наименование приложения</param>
        public void DeleteRolesFromMember(string memberName, string[] roleNames, string appName)
        {
            var roles = _context.Roles.Where(e => e.Application.AppName.Equals(appName, StringComparison.OrdinalIgnoreCase)).Where(e => roleNames.Contains(e.Name));
            var member = _context.Members.Include("Roles").First(r => r.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase));

            foreach (var role in roles)
            {
                member.Roles.Remove(role);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Устанавливает роль для участников безопасности
        /// </summary>
        /// <param name="roleName">Роль</param>
        /// <param name="memberNames">Список участников безопасности</param>
        /// <param name="appName">Наименование приложения</param>
        public void AddMembersToRole(string roleName, string[] memberNames, string appName)
        {
            var members = _context.Members.Where(r => memberNames.Contains(r.Name));
            var role =
                _context.Roles.First(e =>
                        e.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase) &&
                        e.Application.AppName.Equals(appName, StringComparison.OrdinalIgnoreCase));

            foreach (var member in members)
            {
                role.Members.Add(member);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет роль у участников безопасности
        /// </summary>
        /// <param name="roleName">Роль</param>
        /// <param name="memberNames">Список участников безопасности</param>
        /// <param name="appName">Наименование приложения</param>
        public void DeleteMembersFromRole(string roleName, string[] memberNames, string appName)
        {
            var members = _context.Members.Where(r => memberNames.Contains(r.Name));
            var role =
                _context.Roles.Include("Members").First(
                        e => e.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase) &&
                            e.Application.AppName.Equals(appName, StringComparison.OrdinalIgnoreCase));

            foreach (var member in members)
            {
                role.Members.Remove(member);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Добавляет пользователей в группу <see cref="groupName"/>
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="logins"></param>
        public void AddUsersToGroup(string groupName, string[] logins)
        {
            var users = _context.Users.Where(m => logins.Contains(m.Login));
            var group = _context.Groups.First(r => r.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

            foreach (var user in users)
            {
                group.Users.Add(user);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет пользователей из группы
        /// </summary>
        /// <param name="groupName">Группа</param>
        /// <param name="logins">Список пользователей</param>
        public void DeleteUsersFromGroup(string groupName, string[] logins)
        {
            var users = _context.Users.Where(m => logins.Contains(m.Login));
            var group = _context.Groups.Include("Users").First(r => r.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

            foreach (var user in users)
            {
                group.Users.Remove(user);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Добавляет пользователя в группы
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="groupNames">Список групп</param>
        public void AddGroupsToUser(string userName, string[] groupNames)
        {
            var groups = _context.Groups.Where(e => groupNames.Contains(e.Name));
            var user = _context.Users.First(e => e.Login.Equals(userName, StringComparison.OrdinalIgnoreCase));

            foreach (var @group in groups)
            {
                user.Groups.Add(@group);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет пользователя из групп  <see cref="groupNames"/>
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="groupNames">Список групп</param>
        public void DeleteGroupsFromUser(string userName, string[] groupNames)
        {
            var groups = _context.Groups.Where(e => groupNames.Contains(e.Name));
            var user = _context.Users.Include("Groups").First(e => e.Login.Equals(userName, StringComparison.OrdinalIgnoreCase));

            foreach (var @group in groups)
            {
                user.Groups.Remove(@group);
            }

            _context.SaveChanges();
        }

        /// <summary>
        /// Устанвливает пароль пользователю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public bool SetPassword(string login, string password)
        {
            byte[] hashPassword = null;
            if (!string.IsNullOrEmpty(password))
                hashPassword = password.GetMD5HashBytes();

            if (hashPassword != null)
            {
                hashPassword = GetMD5HashPasswordWithSolt(login, hashPassword);
            }

            return _context.Database.ExecuteSqlCommand("exec sec.SetPassword @p0, @p1", login, hashPassword) > 0;
        }

        private byte[] GetMD5HashPasswordWithSolt(string login, byte[] hashPassword)
        {
            var user = _context.Users.First(e => e.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
            hashPassword = hashPassword.Concat(user.PasswordSalt.GetBytes()).ToArray().GetMD5HashBytes();
            return hashPassword;
        }

        /// <summary>
        /// Производит идентификацию пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public bool UserValidate(string login, string password)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(e => e.Login == login);
                if (user == null)
                    throw new UserNotFoundException(login);

                var passwordBytes = GetPassword(login);
                return passwordBytes.SequenceEqual(GetMD5HashPasswordWithSolt(login, password.GetMD5HashBytes())) || (password+user.PasswordSalt).CheckSHA1Hash(passwordBytes);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message">Строка сообщения</param>
        public ILog Log(string message)
        {
            var log = new Log {Message = message};
            _context.Logs.Add(log);
            _context.SaveChanges();

            return log;
        }

        /// <summary>
        /// Возвращает запись из журнала по идентификатору
        /// </summary>
        /// <param name="idLog"></param>
        /// <returns></returns>
        public ILog GetLogById(int idLog)
        {
            return _context.Logs.Single(e => e.IdLog == idLog);
        }

        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message">Строка сообщения</param>
        public async Task<ILog> LogAsync(string message)
        {
            var log = new Log {Message = message};
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return log;

        }

        /// <summary>
        /// Возвращает все объекты безопасности для указанного типа доступа и приложения
        /// </summary>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Наименование приложения</param>
        /// <returns></returns>
        public IEnumerable<string> GetAllowAllSecurityObjects(string accessType, string appName)
        {
            return _context.GetSecurityObjectsForUserByAccessType(accessType, appName);
        }

        /// <summary>
        /// Возвращает разрешенные для пользователя объекты безопасности для указанного типа доступа и приложения
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Наименование приложения</param>
        /// <returns></returns>
        public IEnumerable<string> GetAllowSecurityObjects(string login, string accessType, string appName)
        {
            return _context.GetSecurityObjectsForUserByAccessType(login, accessType, appName, false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }

        #region Helpers

        private byte[] GetPassword(string login)
        {
            var passwordBytes = _context.Database.SqlQuery<byte[]>($"select password from sec.Users where idMember = (select idMember from sec.Members where name = N'{login}')");
            return passwordBytes.First();
        }

        #endregion
    }
}