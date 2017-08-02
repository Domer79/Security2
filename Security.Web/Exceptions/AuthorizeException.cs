using System;

namespace Security.Web.Exceptions
{
    /// <summary>
    /// Происходит при проверке доступа ядром CoreSecurity в атрибуте <see cref="Mvc.AuthorizeAttribute"/>.
    /// Если эта ошибка произошла проверка доступа передается стандартному механизму Mvc
    /// </summary>
    internal class AuthorizeException : Exception
    {
        public AuthorizeException()
        {
        }

        public AuthorizeException(string message)
            : base(message)
        {
            
        }

        public AuthorizeException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}