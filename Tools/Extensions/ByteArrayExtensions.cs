using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Tools.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string GetString(this byte[] bytes)
        {
//            var charArray = Encoding.Unicode.GetChars(bytes);
//            var stringBuilder = new StringBuilder();
//            stringBuilder.Append(charArray);
//            return stringBuilder.ToString();
            return Encoding.Unicode.GetString(bytes);
        }

        public static byte[] GetHashBytes(this string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(value.GetBytes());
        }
    }
}
