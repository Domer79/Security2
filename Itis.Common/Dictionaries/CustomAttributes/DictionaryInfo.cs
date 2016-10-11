using System;

namespace Itis.Common.Dictionaries.CustomAttributes
{
    /// <summary>
    /// Атрибут для описания системного справочника
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class DictionaryInfo : Attribute
    {
        public string Description { get; set; }
        public Type AttributesEnumType { get; set; }
        public EDictionaryCategory Category { get; set; }
        /// <summary>
        /// Порядок в котором будут создаваться системные справочники
        /// </summary>
        /// <remarks>Справочник с атрибутом, имеющим тип <see cref="F:Itis.Common.Dictionaries.EDictionaryAttributeValueType.DictionaryReference"/> 
        /// должен иметь значение порядка большее, чем справочник на который ссылается атрибут</remarks>
        public int CreationOrder { get; set; }

        /// <summary>
        /// Если нет то системный справочник не будет доступен для выбора в Карточке атрибута, нельзя будет создавать элементы справочника, 
        /// и будут недоступны выпадающие списки его элементов
        /// </summary>
        public bool IsActive { get; set; }

        public DictionaryInfo(Type attributesEnumType, EDictionaryCategory category, string description, int creationOrder, bool isActive)
        {
            AttributesEnumType = attributesEnumType;
            Category = category;
            Description = description;
            CreationOrder = creationOrder;
            IsActive = isActive;
        }
    }
}