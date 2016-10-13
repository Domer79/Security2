using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Configurations;
using Security.Interfaces;
using Security.Interfaces.Collections;

namespace Security.Tests
{
    public class Tools : ICheckAccess
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
    }
}
