using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security.Web.Extensions
{
    public static class Identity
    {
        /// <summary>
        /// Возвращает логин, вошедшего в систему пользователя
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string GetLogin(this IIdentity identity)
        {
            return (identity as UserIdentity)?.User.Login ?? identity.Name;
        }
    }
}
