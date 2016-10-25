using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Configurations;

namespace Security
{
    public class BaseSecurity : ISecurity
    {
        private readonly ISecurityTools _securityTools;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public BaseSecurity()
        {
            _securityTools = Config.Get<ISecurityTools>();
        }

        public IUserCollection UserCollection => Config.Get<IUserCollection>();
        public IGroupCollection GroupCollection => Config.Get<IGroupCollection>();
        public ISecObjectCollection SecObjectCollection => Config.Get<ISecObjectCollection>();
        public IRoleCollection RoleCollection => Config.Get<IRoleCollection>();
        public IMemberCollection MemberCollection => Config.Get<IMemberCollection>();

        /// <summary>
        /// Проверка входа
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LogIn(string login, string password)
        {
            return UserCollection.LogIn(login, password);
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
            var accessTypeName = Enum.GetName(Config.AccessType, accessType);
            return _securityTools.CheckAccess(login, secObjectName, accessTypeName);
        }

        public void AddGrant(string roleName, string secObjectName, Enum accessType)
        {
            var accessTypeName = Enum.GetName(Config.AccessType, accessType);
//
            var grantCollection = Config.Get<IGrantCollection>();
            grantCollection.Add(roleName, secObjectName, accessTypeName);
            grantCollection.SaveChanges();
        }

        public void RemoveGrant(string roleName, string secObjectName, Enum accessType)
        {
            var accessTypeName = Enum.GetName(Config.AccessType, accessType);

            var grantCollection = Config.Get<IGrantCollection>();
            grantCollection.Remove(roleName, secObjectName, accessTypeName);
            grantCollection.SaveChanges();
        }

        public void AddRole(string roleName, string memberName)
        {
            _securityTools.AddRoleToMember(roleName, memberName);
        }

        public void DeleteRole(string memberName, string roleName)
        {
            _securityTools.DeleteRoleFromMember(roleName, memberName);
        }
    }
}
