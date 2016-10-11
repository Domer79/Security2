using System;
using System.Xml.Schema;
using Itis.Common;

namespace Itis.ProxyDataPlatform.Base
{
    /// <summary>
    /// Указывает на то что данная сущность представляет отражение членов енума на записи таблицы.
    /// <seealso cref="EnumEntity"/>
    /// </summary>
    public class EnumEntityBll<TEnum> : EntityBll where TEnum : struct, IConvertible
    {
        public EnumEntityBll()
        {
            Validate();
        }

        public EnumEntityBll(string description, string enumName, int enumNumber) : this()
        {
            Description = description;
            EnumName = enumName;
            EnumNumber = enumNumber;
        }

        /// <summary>
        /// значение enum
        /// </summary>
        public int EnumNumber { get; set; }

        /// <summary>
        /// константа enum
        /// </summary>
        public string EnumName { get; set; }

        /// <summary>
        /// Описание на рус которое берется из атрибута enum
        /// </summary>
        public string Description { get; set; }

        public TEnum ConvertToEnum()
        {
            return (TEnum) Enum.Parse(typeof (TEnum), EnumName);
        }

        public void Validate()
        {
            if (!typeof (TEnum).IsEnum)
                throw new Exception("Не указано перечисление для маппинга.");
        }
    }
}