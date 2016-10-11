using System;

namespace Itis.Common.Exceptions
{
    public class DictionaryProviderApiException : Exception
    {
        public DictionaryProviderApiException(string message)
            : base(message)
        {
        }

        public DictionaryProviderApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// ������� ������� ������� �����������, ������� �������� � �������
    /// </summary>
    public class DeletingAttachedDictionaryElement : DictionaryProviderApiException
    {
        public DeletingAttachedDictionaryElement()
            : base("������� ������� ������� �����������, ������� �������� � �������")
        {
        }
    }

    /// <summary>
    /// ������� ������� ������� �����������, ������� ��������� ��� �������� ��������
    /// </summary>
    public class DeletingDictionaryElementAttachedToAttribute : DictionaryProviderApiException
    {
        public DeletingDictionaryElementAttachedToAttribute()
            : base("������� ������� ������� �����������, ������� ��������� � �������� ��������")
        {
        }
    }
}