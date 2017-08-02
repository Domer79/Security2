using System;

namespace Security.Exceptions
{
    /// <summary>
    /// Возникает при синхронизации типов доступа, при удалении более не нужных типов, в случае их отсутствия
    /// </summary>
    public class AccessTypeMissingException : Exception
    {
        public AccessTypeMissingException(string accessTypeName)
            : base($"This is access type {accessTypeName} is missing")
        {
            
        }
    }
}