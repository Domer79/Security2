using System;

namespace Itis.Common.Dictionaries.CustomAttributes
{
    /// <summary>
    /// Кастомный атрибут для описания члена енума, описывающего атрибут какого-то системного справочника, например <see cref="F:Itis.Common.Dictionaries.EAttributeSimCard.PhoneNumber"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class DictionaryProperty : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public EDictionaryAttributeValueType ValueType { get; set; }
        /// <summary>
        /// отображаемое имя атрибута
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Если да то будет прикручена валедация ng-required на веб-морде
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// Указывает на то что значение этого атрибута будет отображаться при представлении элемента справочника на веб морде
        /// </summary>
        public bool IsDisplay { get; set; }
        public int AttributeOrder { get; set; }
        /// <summary>
        /// Когда выбран тип атрибута <see cref="F:Itis.Common.Dictionaries.EDictionaryAttributeValueType.DictionaryReference"/>, указывает на другой системный справочник
        /// </summary>
        public EDictionaryType? ReferenceToSystemDictionary { get; set; }

        /// <summary>
        /// ссылка на исходный атрибут
        /// </summary>
        public Enum SrcDictionaryAttribute { get; set; }

        /// <summary>
        /// Создает атрибут с любыми типами кроме <see cref="F:Itis.Common.Dictionaries.EDictionaryAttributeValueType.DictionaryReference"/>
        /// </summary>
        public DictionaryProperty(string name, string description, EDictionaryAttributeValueType valueType, int attributeOrder, bool isDisplay, bool isRequired)
        {
            Description = description;
            IsDisplay = isDisplay;
            IsRequired = isRequired;
            Name = name;
            ValueType = valueType;
            AttributeOrder = attributeOrder;
        }

        /// <summary>
        /// создает атрибут-ссылку, т.е. значения ограничены элементами другого системного справочника
        /// </summary>
        /// <param name="referenceToSystemDictionary">другой системный тип справочника, элементами которого ограничиваются возможные значения атрибута</param>
        public DictionaryProperty(string name, string description, EDictionaryType referenceToSystemDictionary, bool isRequired, bool isDisplay, int attributeOrder)
        {
            Name = name;
            IsRequired = isRequired;
            IsDisplay = isDisplay;
            ReferenceToSystemDictionary = referenceToSystemDictionary;
            Description = description;
            ValueType = EDictionaryAttributeValueType.DictionaryReference;
            AttributeOrder = attributeOrder;
        }

        public void Validate()
        {
            if (ValueType == EDictionaryAttributeValueType.DictionaryReference && !ReferenceToSystemDictionary.HasValue)
                throw new ArgumentException("Значения атрибута ограничены элементами справочника но справочник не задан");
        }
    }
}