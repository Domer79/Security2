using System;

namespace Itis.ProxyDataPlatform.Exceptions
{
    internal class InvalidConvertOperationException : Exception
    {
        /// <summary>
        /// ¬ыполн€ет инициализацию нового экземпл€ра класса <see cref="T:System.Exception"/>, использу€ указанное сообщение об ошибке.
        /// </summary>
        /// <param name="message">—ообщение, описывающее ошибку.</param>
        public InvalidConvertOperationException(string message) : base(message)
        {
        }

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Exception"/> указанным сообщением об ошибке и ссылкой на внутреннее исключение, которое стало причиной данного исключени€.
        /// </summary>
        /// <param name="message">—ообщение об ошибке с объ€снением причин исключени€.</param><param name="innerException">»сключение, вызвавшее текущее исключение, или указатель null (Nothing в Visual Basic), если внутреннее исключение не задано.</param>
        public InvalidConvertOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.Exception"/>.
        /// </summary>
        public InvalidConvertOperationException()
        {
        }
    }
}