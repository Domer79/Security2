using System;
using System.Runtime.Serialization;

namespace Security.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException()
        {
        }

        public BaseException(string message) 
            : base(message)
        {
        }

        public BaseException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public BaseException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }

    }
}