using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Exceptions;
using Security.Interfaces;
using Security.Interfaces.Collections;
using Security.Model.Entities;
using Security.Configurations;

namespace Security
{
    public class BaseSecurity : ISecurity
    {
        private readonly ICheckAccess _checkAccess;
        private readonly IGrantCollection _grantCollection;
        private readonly IAccessTypeCollection _accessTypeCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public BaseSecurity()
        {
            _grantCollection = Config.Get<IGrantCollection>();
            _accessTypeCollection = Config.Get<IAccessTypeCollection>();
            _checkAccess = Config.Get<ICheckAccess>();

            UserCollection = Config.Get<IUserCollection>();
            GroupCollection = Config.Get<IGroupCollection>();
            SecObjectCollection = Config.Get<ISecObjectCollection>();
            RoleCollection = Config.Get<IRoleCollection>();
            MemberCollection = Config.Get<IMemberCollection>();
        }

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
            return _checkAccess.CheckAccess(login, secObjectName, accessType);
        }

        public void AddGrant(string roleName, string secObjectName, Enum accessType)
        {
            var role = RoleCollection.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
                throw new RoleMissingException(roleName);

            var secObject = SecObjectCollection.FirstOrDefault(so => so.ObjectName == secObjectName);
            if (secObject == null)
                throw new SecObjectMissingException(secObjectName);

            var accessTypeName = Enum.GetName(Config.AccessType, accessType);
            var access = _accessTypeCollection.FirstOrDefault(a => a.Name == accessTypeName);
            if (access == null)
                throw new AccessTypeMissingException(accessTypeName);

            _grantCollection.Add(role, secObject, access);
            _grantCollection.SaveChanges();
        }

        public void RemoveGrant(string roleName, string secObjectName, Enum accessType)
        {
            var role = RoleCollection.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
                throw new RoleMissingException(roleName);

            var secObject = SecObjectCollection.FirstOrDefault(so => so.ObjectName == secObjectName);
            if (secObject == null)
                throw new SecObjectMissingException(secObjectName);

            var accessTypeName = Enum.GetName(Config.AccessType, accessType);
            var access = _accessTypeCollection.FirstOrDefault(a => a.Name == accessTypeName);
            if (access == null)
                throw new AccessTypeMissingException(accessTypeName);

            _grantCollection.Remove(role, secObject, access);
            _grantCollection.SaveChanges();
        }
    }
}
