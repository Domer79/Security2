using System;

namespace Security.EntityFramework.Exceptions
{
    /// <summary>
    /// Происходит при отсутствии в базе данных определенного приложения
    /// </summary>
    public class ApplicationMissingException : Exception
    {
        public ApplicationMissingException(string message)
            : base(message)
        {
            
        }

        public ApplicationMissingException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}