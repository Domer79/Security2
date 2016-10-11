using System.ComponentModel;
using Itis.Common.Dictionaries.CustomAttributes;

namespace Itis.Common.Dictionaries
{
    /// <summary>
    /// Описывает системные типы справочников. Маппится в таблицу SystemDictionaryTypes. Порядок объявления не имеет значения - перед созданием будут отсортированы по <see cref="P:Itis.Common.Dictionaries.CustomAttributes.DictionaryInfo.CreationOrder"/>
    /// </summary>
    /// <remarks>Уже существующую запись в базе можно редактировать, меняя константу и описание члена перечисления но не значение, 
    /// иначе будет сгенерирована ошибка нарушения индекса уникальности IX_UniqueEnumName. Несколько членов с одним значением также недопустимы.
    /// Удаление записи в ответ на удаление члена енума не поддерживается</remarks>
    public enum EDictionaryType
    {
        [Description("Сим карты")]
        [DictionaryInfo(typeof(EAttributeSimCard), EDictionaryCategory.LightingSystem, "справочник телефонов для активации ШУ", 1, true)]
        SIM = 1,
        [Description("GSM-профили")]
        [DictionaryInfo(typeof(EAttributeGSMProfiles), EDictionaryCategory.LightingSystem, "справочник GSM-профилей", 2, true)]
        GSMProfiles = 2
    }
}