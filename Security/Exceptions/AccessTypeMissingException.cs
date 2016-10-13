using System;

namespace Security.Exceptions
{
    public class AccessTypeMissingException : Exception
    {
        public AccessTypeMissingException(string accessTypeName)
            : base($"This is access type {accessTypeName} is missing")
        {
            
        }
    }
}