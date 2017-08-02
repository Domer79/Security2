using System;

namespace Security.Exceptions
{
    /// <summary>
    /// Возникакет при попытке добавить тип доступа, значение строки которого является пустым или null
    /// </summary>
    public class AccessTypeValidException : BaseException
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