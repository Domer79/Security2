using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Security.Extensions;
using Security.Interfaces;
using Itis.Common;

namespace Security.Web
{
    /// <summary>
    /// Модуль аутентификации
    /// </summary>
    public class SecurityAuthenticateHttpModule : IHttpModule
    {
        private readonly string _applicationName;

        public SecurityAuthenticateHttpModule()
        {
            _applicationName = CommonUtils.GetAppSettings("MainApplicationName");
        }

        /// <summary>
        /// Инициализация модуля
        /// </summary>
        /// <param name="context">Элемент <see cref="T:System.Web.HttpApplication"/>, что обеспечивает доступ к методам, свойствам и событиям, общим для всех объектов приложений в ASP.NET приложение</param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Context_AuthenticateRequest;
        }

        /// <summary>
        /// Обрабатывает событие аутентификации приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            var httpContext = ((HttpApplication) sender).Context;
            var user = httpContext.User;

            if (user?.Identity == null)
                return;

            if (!user.Identity.IsAuthenticated)
                return;

            httpContext.User = new UserPrincipal(user.Identity.Name, _applicationName);
        }

        public void Dispose()
        {
            
        }
    }
}
