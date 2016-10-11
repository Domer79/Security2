using System;

namespace Itis.ProxyDataPlatform.Exceptions
{
    public class EntityMapToBllMissingException : Exception
    {
        public EntityMapToBllMissingException()
        {
        }

        public EntityMapToBllMissingException(string message)
            : base(message)
        {
            
        }
    }
}