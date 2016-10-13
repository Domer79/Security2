using System;

namespace Security.Interfaces
{
    public interface ISecurityTools
    {
        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="member">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        bool CheckAccess(string member, string secObjectName, string accessType);

        /// <summary>
        /// Добавляет роль <see cref="roleName"/> участнику <see cref="memberName"/>
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="memberName"></param>
        void AddRoleToMember(string roleName, string memberName);

        void DeleteRoleFromMember(string roleName, string memberName);

        /// <summary>
        /// Добавляет пользователя в группу <see cref="groupName"/>
        /// </summary>
        /// <param name="login"></param>
        /// <param name="groupName"></param>
        void AddUserToGroup(string login, string groupName);

        void DeleteUserFromGroup(string login, string groupName);
    }
}