using System;
using System.Linq;
using System.Reflection;
using Itis.Common.Dictionaries;
using Itis.Common.Dictionaries.CustomAttributes;

namespace Itis.Common.Extensions.Dictionaries
{
    public static class EnumExtensions
    {
        /// <summary>
        /// По заданному атрибуту справочника находит его метаданные 
        /// </summary>
        /// <param name="eDictionaryAttribute">описатель данных атрибута</param>
        /// <returns></returns>
        public static DictionaryProperty GetSystemDictionaryAttributeInfo(this Enum eDictionaryAttribute)
        {
            var prop = GetCustomAttribute<DictionaryProperty>(eDictionaryAttribute);
            if (prop != null)
            {
                prop.SrcDictionaryAttribute = eDictionaryAttribute;
            }
            return prop;
        }

        /// <summary>
        /// Возвращает инфу про системный справочник по его енумовскому индикатору
        /// </summary>
        /// <param name="eDictionary">системный справочник</param>
        /// <returns></returns>
        public static DictionaryInfo GetSystemDictionaryInfo(this Enum eDictionary)
        {
            var dictionaryInfo = GetCustomAttribute<DictionaryInfo>(eDictionary);
            return dictionaryInfo;
        }

        /// <summary>
        /// Возвращает декларативно заданные ограничения на значения атрибута
        /// </summary>
        /// <param name="eRestriction"></param>
        /// <returns></returns>
        public static AttributeValueTypeDetails GetDictionaryAttributeTypeDetails(this Enum eRestriction)
        {
            var restriction = GetCustomAttribute<AttributeValueTypeDetails>(eRestriction);
            return restriction;
        }

        /// <summary>
        /// Возвращает инфу про системный справочник по его енумовскому индикатору
        /// </summary>
        /// <param name="eDictionary">системный справочник</param>
        /// <returns></returns>
        public static TCustomAttr GetCustomAttribute<TCustomAttr>(this Enum eDictionary) where TCustomAttr : class
        {
            Type type = eDictionary.GetType();

            MemberInfo[] memInfo = type.GetMember(eDictionary.ToString());

            if (memInfo != null && memInfo.Length != 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof (TCustomAttr), false);

                if (attrs != null && attrs.Length != 0)
                {
                    return (TCustomAttr) attrs[0];
                }
            }
            return null;
        }

        /// <summary>
        /// По заданному типу справочника находит его свойства и возвращает осортированными по значению
        /// </summary>
        /// <param name="eDictionary">тип справочника</param>
        /// <returns></returns>
        public static DictionaryProperty[] GetOrderedSystemDictionaryAttributes(this EDictionaryType eDictionary)
        {
            Type type = eDictionary.GetType();

            MemberInfo[] memInfo = type.GetMember(eDictionary.ToString());

            if (memInfo != null && memInfo.Length != 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DictionaryInfo), false);

                if (attrs != null && attrs.Length != 0)
                {
                    var info = (DictionaryInfo)attrs[0];
                    DictionaryProperty[] properties = Enum.GetValues(info.AttributesEnumType).Cast<Enum>().Select(GetSystemDictionaryAttributeInfo).OrderBy(x => x.AttributeOrder).ToArray();
                    return properties;
                }
            }
            return new DictionaryProperty[0];
        }
    }
}