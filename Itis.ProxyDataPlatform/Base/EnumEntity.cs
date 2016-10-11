using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itis.ProxyDataPlatform.Base
{
    /// <summary>
    /// Указывает на то что данная сущность представляет таблицу чьи записи отражают члены енума.
    /// </summary>
    /// <remarks>В соответствующем енуме можно обновлять константу, описание но не значение перечисления, иначе будет сгенерирована ошибка нарушения индекса уникальности IX_UniqueEnumName.
    /// Обновление имени константы безопасно и допустимо потому что уникальность константы валидируется на этапе сборки</remarks>
    public class EnumEntity : Entity
    {
        /// <summary>
        /// значение enum 
        /// </summary>
        [Required]
        [Index("IX_UniqueEnumNumber", IsUnique = true, IsClustered = false)]
        public int EnumNumber { get; set; }

        /// <summary>
        /// константа enum
        /// </summary>
        [Required]
        [Index("IX_UniqueEnumName", IsUnique = true, IsClustered = false)]
        [StringLength(100)]
        public string EnumName { get; set; }

        /// <summary>
        /// Описание на рус которое берется из атрибута Description енума
        /// </summary>
        public string Description { get; set; }
    }
}