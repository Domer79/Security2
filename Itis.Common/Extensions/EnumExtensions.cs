using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Itis.Common.Exceptions;

namespace Itis.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Получить значение аттрибута Description
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum _enum)
        {
            Type type = _enum.GetType();

            MemberInfo[] memInfo = type.GetMember(_enum.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return _enum.ToString();
        }

        public static string GetDescription2(this Enum @enum)
        {
            var e = @enum.ToList().First();
            DescriptionAttribute attr = null;
            var fieldInfo = e.GetType().GetField(e.ToString());
            if (Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute)))
                attr = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return attr == null ? e.ToString() : attr.Description;
        }

        public static IEnumerable<Enum> ToList(this Enum @enum)
        {
            return GetEnumList(@enum);
        }

        public static IList<TEnum> ToEnumList<TEnum>()
        {
            if (!typeof(TEnum).Is<Enum>())
                throw new InvalidOperationException("Type must by Enum");

            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>().ToList();
        }

        #region Helpers

        private static IEnumerable<Enum> GetEnumList(Enum @enum)
        {
            var type = @enum.GetType();

            if (Attribute.IsDefined(type, typeof (FlagsAttribute)))
                return GetFlagsEnumList(@enum);

            if (!Validate(@enum))
                throw new EnumValidateException();

            return new[] {@enum};
        }

        private static bool Validate(Enum @enum)
        {
            return @enum.GetType().GetFields().Any(fi => fi.Name == @enum.ToString());
        }

        private static IEnumerable<Enum> GetFlagsEnumList(Enum @enum)
        {
            var values = Enum.GetValues(@enum.GetType());
            var enums = values.OfType<Enum>();

            return enums.Where(@enum.HasFlag);
        }

        #endregion
    }
}