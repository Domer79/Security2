using System;

namespace Itis.ProxyDataPlatform.Exceptions
{
    internal class InvalidConvertOperationException : Exception
    {
        /// <summary>
        /// ��������� ������������� ������ ���������� ������ <see cref="T:System.Exception"/>, ��������� ��������� ��������� �� ������.
        /// </summary>
        /// <param name="message">���������, ����������� ������.</param>
        public InvalidConvertOperationException(string message) : base(message)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Exception"/> ��������� ���������� �� ������ � ������� �� ���������� ����������, ������� ����� �������� ������� ����������.
        /// </summary>
        /// <param name="message">��������� �� ������ � ����������� ������ ����������.</param><param name="innerException">����������, ��������� ������� ����������, ��� ��������� null (Nothing � Visual Basic), ���� ���������� ���������� �� ������.</param>
        public InvalidConvertOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="T:System.Exception"/>.
        /// </summary>
        public InvalidConvertOperationException()
        {
        }
    }
}