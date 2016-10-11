using System;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Itis.Common
{
    public class CommonUtils
    {
        /// <summary>
        /// Возвращает имя переменной/параметра или null если не удалось 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variableOrParameter">Выражение типа () => param1 например</param>
        /// <remarks>Можно использовать при генерации <see cref="ArgumentException"/> например или для логирования</remarks>
        public static string GetVariableOrParameterName<T>(Expression<Func<T>> variableOrParameter)
        {
            if (variableOrParameter != null)
            {
                var expressionBody = variableOrParameter.Body as MemberExpression;
                if (expressionBody != null)
                    return expressionBody.Member.Name;
            }
            return null;
        }

        /// <summary>
        /// Получить размер файла в байтах
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <returns></returns>
        public static long? GetFileSize(string filePath)
        {
            var fi = new FileInfo(filePath);

            if (fi.Exists)
            {
                return fi.Length;
            }

            return null;
        }

        /// <summary>
        /// Получить размер файла в байтах
        /// </summary>
        /// <param name="file">массив byte</param>
        /// <returns></returns>
        public static long? GetFileSize(byte[] file)
        {
            
            if (file != null)
                return file.Length;
            
            return null;
        }

        /// <summary>
        /// Вычисление контрольной суммы md5
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <returns></returns>
        public static string GetMd5HashFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Вычисление контрольной суммы md5
        /// </summary>
        /// <param name="file">массив byte</param>
        /// <returns></returns>
        public static string GetMd5HashFromFile(byte[] file)
        {
            if (file != null)
            {
                using (var md5 = MD5.Create())
                {
                    return BitConverter.ToString(md5.ComputeHash(file)).Replace("-", string.Empty);
                }
            }

            return "";
        }

    }
}