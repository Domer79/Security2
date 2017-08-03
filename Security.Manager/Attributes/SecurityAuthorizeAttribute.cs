using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Security.Manager.Attributes
{
    public class SecurityAuthorizeAttribute : Security.Web.Mvc.AuthorizeAttribute
    {
        public SecurityAuthorizeAttribute(string objectName) 
            : base(typeof(SecurityAuthorizeAttribute).Assembly)
        {
            if (string.IsNullOrWhiteSpace(objectName))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(objectName));

            ObjectName = objectName;
        }

        /// <summary>
        /// Возвращает страницу с сообщением об ошибке на обычный http запрос
        /// </summary>
        /// <returns>ActionResult</returns>
        protected override ActionResult GetNotAllowViewResult()
        {
            return new HttpUnauthorizedResult("Access Denied!");
        }

        /// <summary>
        /// Возвращает сообщение об ошибки на AJAX запрос
        /// </summary>
        /// <returns>Сообщение в формате JSON, XML или в любых других форматах</returns>
        protected override ActionResult GetNotAllowAjaxResult()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied!");
        }
    }
}