using System;

namespace Security.EntityDal
{
    /// <summary>
    /// Является оберткой для внутренних исключений при работе с базой данных
    /// </summary>
    public class SecurityConfigurationSetException : Exception
    {
        public SecurityConfigurationSetException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}