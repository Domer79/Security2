using System;

namespace Security.Exceptions
{
    public class SecObjectMissingException : Exception
    {
        public SecObjectMissingException(string secObjectName)
            : base($"This is security object {secObjectName} is missing")
        {
            
        }
    }
}