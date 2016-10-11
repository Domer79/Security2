using System;
using System.Xml.Schema;
using Itis.Common;

namespace Itis.ProxyDataPlatform.Base
{
    /// <summary>
    /// ��������� �� �� ��� ������ �������� ������������ ��������� ������ ����� �� ������ �������.
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
        /// �������� enum
        /// </summary>
        public int EnumNumber { get; set; }

        /// <summary>
        /// ��������� enum
        /// </summary>
        public string EnumName { get; set; }

        /// <summary>
        /// �������� �� ��� ������� ������� �� �������� enum
        /// </summary>
        public string Description { get; set; }

        public TEnum ConvertToEnum()
        {
            return (TEnum) Enum.Parse(typeof (TEnum), EnumName);
        }

        public void Validate()
        {
            if (!typeof (TEnum).IsEnum)
                throw new Exception("�� ������� ������������ ��� ��������.");
        }
    }
}