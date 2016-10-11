using System.ComponentModel;
using Itis.Common.Dictionaries.CustomAttributes;

namespace Itis.Common.Dictionaries
{
    /*
     * Используется для обозначения типа значений атрибутов как системных справочников так и пользовательских.
     * Иногда нужно будет сделать так что тип предназначен для атрибутов только системных справочников, тогда ставить кастомный атрибут AttributeValueType с IsPrivate = true обязательно
    */

    /// <summary>
    /// Описывает тип значения атрибута. Нужен для валидации значений.  
    /// Маппится в DictionaryAttributeValueTypes
    /// </summary>
    /// <remarks>Уже существующую запись в базе можно редактировать, меняя константу и описание члена перечисления но не значение, 
    /// иначе будет сгенерирована ошибка нарушения индекса уникальности IX_UniqueEnumName. Несколько членов с одним значением также недопустимы.
    /// Удаление члена не отразится в базу</remarks>
    public enum EDictionaryAttributeValueType
    {
        [Description("Число")]
        Number = 1,
        [Description("Строка")]
        [AttributeValueTypeDetails(false)]
        String = 2,
        /// <summary>
        /// Когда значениями атрибута выступают элементы другого справочника
        /// </summary>
        [Description("Ссылка на элемент справочника")]
        [AttributeValueTypeDetails(false)]
        DictionaryReference = 3,
        [Description("Чекбокс")]
        [AttributeValueTypeDetails(false)]
        Boolean
    }
}