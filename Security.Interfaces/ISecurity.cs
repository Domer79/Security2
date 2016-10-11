using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces.Collections;

namespace Security.Interfaces
{
    public interface ISecurity
    {
        IUserCollection UserCollection { get; }
        IGroupCollection GroupCollection { get; }
        ISecObjectCollection SecObjectCollection { get; }
        IRoleCollection RoleCollection { get; }
        IMemberCollection MemberCollection { get; }

        /// <summary>
        /// Проверка входа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool LogIn(string login, string password);

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        bool CheckAccess(string login, string secObjectName, string accessType);

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        bool CheckAccess(string login, string secObjectName, Enum accessType);
    }
}
