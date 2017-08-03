using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Представляет коллекцию объектов в виде строки, разделенных условным разделителем
        /// </summary>
        /// <param name="args">Коллекция объектов</param>
        /// <param name="delimiter">Условный разделитель</param>
        /// <returns></returns>
        public static string SplitReverse(this IEnumerable<object> args, string delimiter)
        {
            if (args == null)
                return String.Empty;

            var enumerable = args as object[] ?? args.ToArray();
            var splitResult = !enumerable.Any() ? String.Empty : enumerable.Select(arg => arg.ToString()).Aggregate((str, next) => str + delimiter + next);
            return splitResult;
        }

        /// <summary>
        /// Представляет коллекцию объектов в виде строки, разделенных запятой
        /// </summary>
        /// <param name="args">Коллекция объектов</param>
        /// <returns></returns>
        public static string SplitReverse(this IEnumerable<object> args)
        {
            return SplitReverse(args, ", ");
        }

        /// <summary>
        /// Преобразует строку в массив байтов
        /// </summary>
        /// <param name="value">Исходная строка</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string value)
        {
            return Encoding.Unicode.GetBytes(value);
        }

        /// <summary>
        /// Проверяет, совпадает ли строка определенному шаблону
        /// </summary>
        /// <param name="value">Исходная строка</param>
        /// <param name="pattern">Шаблон</param>
        /// <returns></returns>
        public static bool RxIsMatch(this string value, string pattern)
        {
            return RxIsMatch(value, pattern, RegexOptions.None);
        }

        /// <summary>
        /// Проверяет, совпадает ли строка определенному шаблону
        /// </summary>
        /// <param name="value">Исходная строка</param>
        /// <param name="pattern">Шаблон</param>
        /// <param name="options">Опции для шаблона регулярного выражения</param>
        /// <returns></returns>
        public static bool RxIsMatch(this string value, string pattern, RegexOptions options)
        {
            var rx = new Regex(pattern);
            return rx.IsMatch(value);
        }
    }
}
