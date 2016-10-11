using Itis.Common.Dictionaries.CustomAttributes;

namespace Itis.Common.Dictionaries
{
    /// <summary>
    /// Атрибуты системного справочника <see cref="F:Itis.Common.Dictionaries.EDictionaryType.SIM"/>. Порядок объявления не имеет значения.
    /// </summary>
    public enum EAttributeSimCard
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        [DictionaryProperty("Номер телефона", null, EDictionaryAttributeValueType.String, 1, true, true)]
        PhoneNumber = 1,

        [DictionaryProperty("ICCID СИМ-карты", null, EDictionaryAttributeValueType.String, 2, true, false)]
        ICCID = 2,
    }

    /// <summary>
    /// Атрибуты системного справочника <see cref="F:Itis.Common.Dictionaries.EDictionaryType.GSMProfiles"/>. Порядок объявления не имеет значения.
    /// </summary>
    public enum EAttributeGSMProfiles
    {
        /// <summary>
        /// Оператор
        /// </summary>
        [DictionaryProperty("Оператор", null, EDictionaryAttributeValueType.String, 1, true, false)]
        Operator = 1,

        /// <summary>
        /// Телефон СМС центра
        /// </summary>
        [DictionaryProperty("СМС центр", null, EDictionaryAttributeValueType.String, 3, true, false)]
        SmsCenter = 2,

        /// <summary>
        /// Адрес точки доступа
        /// </summary>
        [DictionaryProperty("Точка доступа", null, EDictionaryAttributeValueType.String, 4, true, false)]
        AccessPoint = 3,

        /// <summary>
        /// Логин
        /// </summary>
        [DictionaryProperty("Логин", null, EDictionaryAttributeValueType.String, 5, true, false)]
        Login = 4,

        /// <summary>
        /// Пароль
        /// </summary>
        [DictionaryProperty("Пароль", null, EDictionaryAttributeValueType.String, 6, true, false)]
        Password = 5,
        
        /// <summary>
        /// Имя
        /// </summary>
        [DictionaryProperty("Имя", null, EDictionaryAttributeValueType.String, 2, true, true)]
        Name = 6
    }
}