using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Tools.Extensions
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Сериализует объект в бинарный формат
        /// </summary>
        /// <param name="object"></param>
        /// <returns>Массив байтов</returns>
        public static byte[] Serialize(this object @object)
        {
            if (@object == null) 
                throw new ArgumentNullException("object");

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, @object);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// Сериализует объект в бинарный формат и возвращает полученный массив байтов в виде строки
        /// </summary>
        /// <param name="object">Объект для сериализации</param>
        /// <returns>Сериализованная строка</returns>
        public static string SerializeToString(this object @object)
        {
            return @object.Serialize().GetString();
        }

        /// <summary>
        /// Десериализует объект из входной строки
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="data">Входная строка - объект, представленный в виде строки</param>
        /// <returns>Экземпляр объекта</returns>
        public static T DeserializeFromString<T>(string data)
        {
            return (T)DeserializeFromString(data);
        }

        /// <summary>
        /// Десериализует объект из входной строки
        /// </summary>
        /// <param name="data">Входная строка - объект, представленный в виде строки</param>
        /// <returns>Экземпляр объекта</returns>
        public static object DeserializeFromString(string data)
        {
            return Deserialize(Encoding.Unicode.GetBytes(data));
        }

        /// <summary>
        /// Десериализует объект из массива байтов
        /// </summary>
        /// <typeparam name="T">Тип объектка</typeparam>
        /// <param name="data">Массив байтов</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            return (T)Deserialize(data);
        }

        /// <summary>
        /// Десериализует объект из массива байтов
        /// </summary>
        /// <param name="data">Массив байтов</param>
        /// <returns></returns>
        public static object Deserialize(byte[] data)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                return bf.Deserialize(ms);
            }
        }
    }
}
