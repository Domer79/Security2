using System;

namespace Itis.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static T ParseToEnum<T>(this string expectedEnum)
        {
            return (T) Enum.Parse(typeof (T), expectedEnum);
        }
    }
}