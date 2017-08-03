using System;

namespace Security.EntityFramework.Exceptions
{
    /// <summary>
    /// Исключение, возникает при неудачном поиске пользователя в коллекции базы данных
    /// </summary>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string login)
            : base($"User {login} not found!")
        {
            
        }
    }
}