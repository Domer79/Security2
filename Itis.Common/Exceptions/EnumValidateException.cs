using System;

namespace Itis.Common.Exceptions
{
    internal class EnumValidateException : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="T:System.Exception"/>.
        /// </summary>
        public EnumValidateException()
        {
        }

        /// <summary>
        /// Выполняет инициализацию нового экземпляра класса <see cref="T:System.Exception"/>, используя указанное сообщение об ошибке.
        /// </summary>
        /// <param name="message">Сообщение, описывающее ошибку.</param>
        public EnumValidateException(string message)
            : base(message)
        {
        }
    }
}