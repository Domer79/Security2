using System;

namespace Security.EntityFramework.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string login)
            : base($"User {login} not found!")
        {
            
        }
    }
}