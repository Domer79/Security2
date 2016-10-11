using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Itis.ProxyDataPlatform.DatabaseContext;
using Itis.ProxyDataPlatform.Interfaces;

namespace Itis.ProxyDataPlatform.Converting
{
    public class ConverterCollection : IEnumerable<IConverter>
    {
        private readonly Dictionary<Type, IConverter> _converters = new Dictionary<Type, IConverter>();

        private void Add(Type entityType, IConverter converter)
        {
            if (_converters.ContainsKey(entityType))
                return;

            _converters.Add(entityType, converter);
        }

        internal IConverter this[Type type]
        {
            get { return _converters[type]; }
        }

        public int Count
        {
            get { return _converters.Count; }
        }

        /// <summary>
        /// Возвращает перечислитель, выполняющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Интерфейс <see cref="T:System.Collections.Generic.IEnumerator`1"/>, который может использоваться для перебора элементов коллекции.
        /// </returns>
        IEnumerator<IConverter> IEnumerable<IConverter>.GetEnumerator()
        {
            return _converters.Values.GetEnumerator();
        }

        /// <summary>
        /// Возвращает перечислитель, осуществляющий итерацию в коллекции.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.Collections.IEnumerator"/>, который может использоваться для перебора коллекции.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IConverter>)this).GetEnumerator();
        }

        #region Helpers

//        private static void InitByInterface(ConverterCollection collection, Assembly assembly)
//        {
//            var converterTypes = assembly.GetTypes().Where(IsIConverter);
//            foreach (var converterType in converterTypes)
//            {
//                var entityType = converterType.GetInterface("iconverter`2", true).GetGenericArguments()[0];
//                var converter = (IConverter) Activator.CreateInstance(converterType);
//                collection.Add(entityType, converter);
//            }
//        }

        private static void InitByBase(ConverterCollection collection, Assembly assembly)
        {
            var converterTypes = assembly.GetTypes().Where(IsBaseConverter);
            foreach (var converterType in converterTypes)
            {
                // ReSharper disable once PossibleNullReferenceException
                var entityType = converterType.BaseType.GetGenericArguments()[0];
                var converter = (IConverter)Activator.CreateInstance(converterType);
                collection.Add(entityType, converter);
            }
        }

//        private static bool IsIConverter(Type t)
//        {
//            var iconverterType = t.GetInterface("iconverter`2", true);
//            if (iconverterType == null)
//                return false;
//
//            return iconverterType.GetGenericTypeDefinition() == typeof (IConverter<,>);
//        }

        private static bool IsBaseConverter(Type t)
        {
            if (t.GetInterface("iconverter", true) == null)
                return false;

            if (t.BaseType == null)
                return false;

            if (!t.BaseType.IsGenericType)
                return false;

            if (t.BaseType.GetGenericTypeDefinition() != typeof (BaseConverter<,>)) 
                return false;

            if (t.IsGenericType)
                return false;

            return true;
        }

        #endregion

        public void AddConverterAssembly(Assembly assembly)
        {
            InitByBase(this, assembly);
        }
    }
}
