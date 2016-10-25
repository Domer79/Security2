using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Security;
using Security.Configurations;
using Security.EntityFramework;
using Security.FakeData.Common;
using Security.FakeData.Model;
using Tools.Extensions;

namespace Itis.Suo.App_Start
{
    public class SecurityConfig
    {
        private static BaseSecurity _security;

        public static BaseSecurity Security
        {
            get { return _security; }
        }

        public static void RegisterSecurity()
        {
            Config.RegisterCommonModule<CommonModule>();
            Config.RegisterAccessTypes(typeof(EAccessType));
            _security = new BaseSecurity();

            #region  Разархивировать для FakeCommonModule
            //            Security.UserCollection.Add(new User() { Login = "User1", Password = "password".GetHashBytes() });
            //            Security.UserCollection.SaveChanges();
            //            Security.RoleCollection.Add(new Role() { Name = "Role1" });
            //            Security.RoleCollection.SaveChanges();
            //            Security.SecObjectCollection.Add(new SecObject() { ObjectName = "SecObject1" });
            //            Security.SecObjectCollection.SaveChanges();
            //            Security.AddGrant("Role1", "SecObject1", EAccessType.Select);
            //            Security.AddRole("Role1", "User1");
            #endregion
        }
    }

    public enum EAccessType
    {
        Add,
        Update,
        Delete,
        Select
    }
}