using System.Security.Cryptography;
using System.Text;

namespace Tools.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Возвращает строку из массива байтов
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes)
        {
//            var charArray = Encoding.Unicode.GetChars(bytes);
//            var stringBuilder = new StringBuilder();
//            stringBuilder.Append(charArray);
//            return stringBuilder.ToString();
            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// Возвращает хэш MD5 введенной строки 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetMD5HashBytes(this string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(value.GetBytes());
        }

        /// <summary>
        /// Возвращает хэш MD5 массива байтов
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] GetMD5HashBytes(this byte[] bytes)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(bytes);
        }

        /// <summary>
        /// Возвращает хэш SHA1 введенной строки 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetSHA1HashBytes(this string value)
        {
            var md5 = new SHA1CryptoServiceProvider();
            return md5.ComputeHash(value.GetBytes());
        }
        
        /// <summary>
        /// Проверка строки хеша с другим хэшем представленным в виде массива байтов
        /// </summary>
        /// <param name="data">Строка хэша SHA1</param>
        /// <param name="hash">Массив байтов хэша SHA1</param>
        /// <returns></returns>
        public static bool CheckSHA1Hash(this string data, byte[] hash)
        {
            if (data == null || hash == null)
                return false;
            var datahash = data.GetSHA1HashBytes();
            if (datahash.Length != hash.Length)
                return false;
            for (int i = 0; i < hash.Length; i++)
                if (datahash[i] != hash[i])
                    return false;
            return true;
        }

    }
}
