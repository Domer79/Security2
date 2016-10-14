using System;

namespace Security.Exceptions
{
    internal class AccessTypeValidException : BaseException
    {
        public AccessTypeValidException(string message, params Type[] args)
            : base(message, args)
        {
        }

        public AccessTypeValidException(string accessName)
            : base("AccessName is null or empty. AccessName: {0}", accessName)
        {

        }
    }
}