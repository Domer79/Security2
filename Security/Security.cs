using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces;
using Security.Interfaces.Collections;

namespace Security
{
    public class Security : ISecurity
    {
        public IUserCollection UserCollection { get; }
        public IGroupCollection GroupCollection { get; }
        public ISecObjectCollection SecObjectCollection { get; }
        public IRoleCollection RoleCollection { get; }
        public IMemberCollection MemberCollection { get; }

        /// <summary>
        /// Проверка входа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LogIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        public bool CheckAccess(string login, string secObjectName, string accessType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверка доступа
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="secObjectName">Объект безопасности</param>
        /// <param name="accessType">Тип доступа</param>
        /// <returns></returns>
        public bool CheckAccess(string login, string secObjectName, Enum accessType)
        {
            throw new NotImplementedException();
        }
    }
}
