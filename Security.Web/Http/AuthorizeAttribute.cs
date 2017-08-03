using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Security.Configurations;
using Security.Interfaces;
using Security.Web.Exceptions;
using Security.Exceptions;

namespace Security.Web.Http
{
    /// <summary>
    /// Абстрактный класс атрибута авторизации. Осуществляет проверку авторизации пользователя
    /// </summary>
    public abstract class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute, ISecurityObject
    {
        private string _controller;
        private string _action;
        private readonly string _applicationName;
        private string _accessType = Config.Exec;

        protected AuthorizeAttribute(string applicationName)
        {
            _applicationName = applicationName;
        }

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="callAssembly"></param>
        /// <exception cref="SecurityNotSupportedException"></exception>
        protected AuthorizeAttribute(Assembly callAssembly)
        {
            if (callAssembly == null)
                throw new ArgumentNullException(nameof(callAssembly));

            var productAttribute = callAssembly.GetCustomAttribute<AssemblySecurityApplicationInfoAttribute>();

            if (productAttribute == null)
                throw new SecurityNotSupportedException("Отсутствует информация о приложении");

            _applicationName = productAttribute.ApplicationName;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException(nameof(actionContext));

            var principal = actionContext.ControllerContext.RequestContext.Principal;
            if (!principal.Identity.IsAuthenticated)
                return false;

            _action = actionContext.ActionDescriptor.ActionName;
            _controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;

            using (var security = new CoreSecurity(_applicationName))
            {
                var login = ((UserIdentity)principal.Identity).User.Login;
                return security.CheckAccess(login, ((ISecurityObject)this).ObjectName ?? Mvc.AuthorizeAttribute.GetObjectName(_controller, _action), Config.Exec, _applicationName);
            }
        }

        /// <summary>
        /// Наименование объекта безопасности, для которого требуется запрашивать разрешение на доступ
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Тип доступа для объекта безопасности
        /// </summary>
        public string AccessType
        {
            get { return _accessType; }
            set { _accessType = Config.Exec; }
        }
    }
}
