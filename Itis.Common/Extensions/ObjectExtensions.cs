using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Itis.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool Is(this object @object, Type parentType)
        {
            if (@object == null)
                throw new ArgumentNullException("object");

            return @object.GetType().Is(parentType);
        }

        public static bool Is<T>(this object @object)
        {
            if (@object == null)
                throw new ArgumentNullException("object");

            var type = @object as Type;
            if (type != null)
                return type.Is<T>();

            return @object.Is(typeof(T));
        }

        public static T As<T>(this object @object)
            where T : class
        {
            if (@object.Is<T>())
                return (T)@object;

            return null;
        }

        public static string ToJsonString(this object @object)
        {
            try
            {
                var jsonSerializeSettings = new JsonSerializerSettings();
                jsonSerializeSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                jsonSerializeSettings.TypeNameHandling = TypeNameHandling.Objects;
                var serializer = JsonSerializer.Create(jsonSerializeSettings);
//                var serializer = JsonSerializer.Create();
                var sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    serializer.Serialize(sw, @object);
                }

                return sb.ToString();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Не удалось сериализовать объект. Подробности во внутреннем исключении.", e);
            }
        }

        public static T FromJsonString<T>(this string inputString)
        {
            try
            {
                var serializer = new JsonSerializer();

                using (TextReader tr = new StringReader(inputString))
                {
                    using (JsonReader jr = new JsonTextReader(tr))
                    {
                        return serializer.Deserialize<T>(jr);
                    }
                }

            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Не удалось десериализовать объект. Подробности во внутреннем исключении.", e);
            }
        }

        //todo: Вернуться позже к реализации
        private static void AssignTo(this object source, object dest)
        {

        }

        //todo: Вернуться позже к реализации
        private static void AssignTo(object source, object dest, IEnumerable<object> cycleObjects)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (dest == null)
                throw new ArgumentNullException("dest");

            var properties = dest.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(source);
            }
        }
    }
}
