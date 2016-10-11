using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Itis.Common.Extensions
{
    public static class TypeExtensions
    {
        private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

        public static IEnumerable<Type> GetParentTypes(this Type type)
        {
            while (type != null)
            {
                var t = type;
                type = type.BaseType;
                yield return t;
            }
        }

        public static bool Is(this Type type, Type parentType)
        {
            return GetParentTypes(type).Any(t => t == parentType);
        }

        public static bool Is<T>(this Type type)
        {
            return Is(type, typeof(T));
        }

        public static MethodInfo GetGenericMethod(this Type type, string methodName, Type[] genericArguments,
            Type[] parameterTypes)
        {
            return GetGenericMethod(type, methodName, DefaultLookup, genericArguments, parameterTypes);
        }

        public static MethodInfo GetGenericMethod(this Type type, string methodName, BindingFlags bindingFlags, Type[] genericArguments,
            Type[] parameterTypes)
        {
            var methods = type.GetMethods(bindingFlags).Where(m => m.IsGenericMethodDefinition).Where(m => m.Name == methodName);
            MethodInfo resultMethod = null;

            foreach (var methodInfo in methods)
            {
                resultMethod = methodInfo.MakeGenericMethod(genericArguments);
                var parameters = resultMethod.GetParameters();

                if (!parameters.Select(p => p.ParameterType).SequenceEqual(parameterTypes))
                {
                    resultMethod = null;
                    continue;
                }
            }

            return resultMethod;
        }

        public static bool IsPrimitive(this Type type)
        {
            return PrimitiveTypes.Test(type);
        }

        public static Type GetInterface<TInterface>(this Type type) where TInterface : class
        {
            var interfaceName = typeof (TInterface).Name;

            return type.GetInterface(interfaceName);
        }

        public static bool ExistInterface<TInterface>(this Type type) where TInterface : class
        {
            var t = type.GetInterface<TInterface>();

            return t != null;
        }

        #region Helpers

        /// <summary>
        /// Взято с: http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive
        /// </summary>
        private static class PrimitiveTypes
        {
            private static readonly Type[] List;

            static PrimitiveTypes()
            {
                var types = new[]
                {
                    typeof (Enum),
                    typeof (String),
                    typeof (Char),
                    typeof (Guid),

                    typeof (Boolean),
                    typeof (Byte),
                    typeof (Int16),
                    typeof (Int32),
                    typeof (Int64),
                    typeof (Single),
                    typeof (Double),
                    typeof (Decimal),

                    typeof (SByte),
                    typeof (UInt16),
                    typeof (UInt32),
                    typeof (UInt64),

                    typeof (DateTime),
                    typeof (DateTimeOffset),
                    typeof (TimeSpan),
                };


                var nullTypes = from t in types
                                where t.IsValueType
                                select typeof(Nullable<>).MakeGenericType(t);

                List = types.Concat(nullTypes).ToArray();
            }

            public static bool Test(Type type)
            {
                if (List.Any(x => x.IsAssignableFrom(type)))
                    return true;

                var nut = Nullable.GetUnderlyingType(type);
                return nut != null && nut.IsEnum;
            }
        }

        #endregion
    }
}