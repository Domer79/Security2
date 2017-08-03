using System;
using System.Linq;
using System.Security.Principal;

namespace Security.Web
{
    /// <summary>
    /// Представляет контекст безопасности пользователя, от лица которого выполняется код, 
    /// включая удостоверение пользователя (IIdentity) и любые принадлежащие ему роли.
    /// </summary>
    public class UserPrincipal : IPrincipal
    {
        private readonly string _login;
        private readonly string _applicationName;

        public UserPrincipal(string login, string applicationName)
        {
            _login = login;
            _applicationName = applicationName;
        }

        /// <summary>
        /// Проверяет наличие у пользователя роли <see cref="role"/>
        /// </summary>
        /// <param name="role">Проверяемая роль</param>
        /// <returns>True, если пользователь имеет данную роль, иначе False</returns>
        public bool IsInRole(string role)
        {
            using (var security = new CoreSecurity(_applicationName))
            {
                var roles = security.MemberCollection.WithRoles()
                    .Where(u => u.Name == _login)
                    .SelectMany(m => m.Roles);

                return roles.Any(e => e.Name.Equals(role, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Удостоверение пользователя <see cref="UserIdentity"/>
        /// </summary>
        public IIdentity Identity => new UserIdentity(_login, _applicationName);

        public override string ToString()
        {
            return Identity.ToString();
        }
    }
}