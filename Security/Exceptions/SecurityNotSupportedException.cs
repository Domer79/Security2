using System;

namespace Security.Exceptions
{
    /// <summary>
    /// Возникает при проверке сборки на наличие поддержки модели безопасности
    /// </summary>
    public class SecurityNotSupportedException : Exception
    {
        public SecurityNotSupportedException(string message)
            :base(message)
        {
            
        }
    }
}