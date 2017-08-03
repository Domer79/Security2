using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Security.Interfaces.Model;
using System.Collections.Generic;

namespace Security.Interfaces
{
    /// <summary>
    /// Дополнительный инструментарий для ядра системы доступа
    /// </summary>
    public interface ISecurityTools : IDisposable
    {
        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="member">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName"></param>
        /// <returns></returns>
        bool CheckAccess(string member, string secObjectName, string accessType, string appName);

        /// <summary>
        /// Добавляет роли <see cref="roleNames"/> участнику <see cref="memberName"/>
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="appName">Наименование приложения</param>
        /// <param name="roleNames"></param>
        void AddRolesToMember(string memberName, string[] roleNames, string appName);

        /// <summary>
        /// Удаляет роли у участника безопасности
        /// </summary>
        /// <param name="memberName">Участник безопасности</param>
        /// <param name="roleNames">Список ролей</param>
        /// <param name="appName">Наименование приложения</param>
        void DeleteRolesFromMember(string memberName, string[] roleNames, string appName);

        /// <summary>
        /// Устанавливает роль для участников безопасности
        /// </summary>
        /// <param name="roleName">Роль</param>
        /// <param name="memberNames">Список участников безопасности</param>
        /// <param name="appName">Наименование приложения</param>
        void AddMembersToRole(string roleName, string[] memberNames, string appName);

        /// <summary>
        /// Удаляет роль у участников безопасности
        /// </summary>
        /// <param name="roleName">Роль</param>
        /// <param name="memberNames">Список участников безопасности</param>
        /// <param name="appName">Наименование приложения</param>
        void DeleteMembersFromRole(string roleName, string[] memberNames, string appName);

        /// <summary>
        /// Добавляет пользователей в группу <see cref="groupName"/>
        /// </summary>
        /// <param name="groupName">Группа</param>
        /// <param name="logins">Список пользователей</param>
        void AddUsersToGroup(string groupName, string[] logins);

        /// <summary>
        /// Удаляет пользователей из группы
        /// </summary>
        /// <param name="groupName">Группа</param>
        /// <param name="logins">Список пользователей</param>
        void DeleteUsersFromGroup(string groupName, string[] logins);

        /// <summary>
        /// Добавляет пользователя в группы
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="groupNames">Список групп</param>
        void AddGroupsToUser(string userName, string[] groupNames);

        /// <summary>
        /// Удаляет пользователя из групп  <see cref="groupNames"/>
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="groupNames">Список групп</param>
        void DeleteGroupsFromUser(string userName, string[] groupNames);

        /// <summary>
        /// Устанвливает пароль пользователю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        bool SetPassword(string login, string password);

        /// <summary>
        /// Производит идентификацию пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        bool UserValidate(string login, string password);

        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message">Строка сообщения</param>
        ILog Log(string message);

        /// <summary>
        /// Возвращает запись из журнала по идентификатору
        /// </summary>
        /// <param name="idLog"></param>
        /// <returns></returns>
        ILog GetLogById(int idLog);

        /// <summary>
        /// Запись сообщения в лог
        /// </summary>
        /// <param name="message">Строка сообщения</param>
        Task<ILog> LogAsync(string message);

        /// <summary>
        /// Возвращает разрешенные для пользователя объекты безопасности с указанием типа доступа
        /// </summary>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Наименование приложения</param>
        /// <returns></returns>
        IEnumerable<string> GetAllowAllSecurityObjects(string accessType, string appName);

        /// <summary>
        /// Возвращает разрешенные для пользователя объекты безопасности с указанием типа доступа
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="accessType">Тип доступа</param>
        /// <param name="appName">Наименование приложения</param>
        /// <returns></returns>
        IEnumerable<string> GetAllowSecurityObjects(string login, string accessType, string appName);
    }
}