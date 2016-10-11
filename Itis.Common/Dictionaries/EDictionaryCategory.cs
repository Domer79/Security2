using System.ComponentModel;

namespace Itis.Common.Dictionaries
{
    /// <summary>
    /// Группировщик справочников по пренадлежности. Например "Система освещения" или "Устройства самообслуживания".
    /// Маппится в таблицу DictionaryCategories
    /// </summary>
    /// <remarks>Уже существующую запись в базе можно редактировать, меняя константу и описание члена перечисления но не значение, 
    /// иначе будет сгенерирована ошибка нарушения индекса уникальности IX_UniqueEnumName. Несколько членов с одним значением также недопустимы</remarks>
    public enum EDictionaryCategory
    {
        [Description("Система освещения")]
        LightingSystem = 1,
        [Description("Устройства самообслуживания")]
        SelfServiceTerminalSystem = 2,
        [Description("Без категории")]
        None = 3
    }
}