using System;

namespace Security.Exceptions
{
    public class RoleMissingException : Exception
    {
        public RoleMissingException(string roleName)
            : base($"This is role {roleName} is missing")
        {
            
        }
    }
}