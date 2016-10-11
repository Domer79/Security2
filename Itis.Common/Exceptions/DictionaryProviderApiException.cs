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
    /// Попытка удалить элемент справочника, который привязан к объекту
    /// </summary>
    public class DeletingAttachedDictionaryElement : DictionaryProviderApiException
    {
        public DeletingAttachedDictionaryElement()
            : base("Попытка удалить элемент справочника, который привязан к объекту")
        {
        }
    }

    /// <summary>
    /// Попытка удалить элемент справочника, который выставлен как значение атрибута
    /// </summary>
    public class DeletingDictionaryElementAttachedToAttribute : DictionaryProviderApiException
    {
        public DeletingDictionaryElementAttachedToAttribute()
            : base("Попытка удалить элемент справочника, который выставлен в значении атрибута")
        {
        }
    }
}