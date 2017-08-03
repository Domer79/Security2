using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Security.EntityDal.Infrastructure
{
    /// <summary>
    /// Класс информатор, отображающий некоторые сведения о контексте базы данных
    /// </summary>
    public class ContextInfo
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="context">Экземпляр контекста БД</param>
        public ContextInfo(DbContext context)
            : this(context.GetType())
        {
            
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="contextType">Тип контекста БД</param>
        public ContextInfo(Type contextType)
        {
        }

        internal string DatabaseName { get; set; }

        /// <summary>
        /// Возвращает свойства сущностей контекста БД
        /// </summary>
        /// <param name="contextType">Тип контекста</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetDbSetProperties(Type contextType)
        {
            var enumerable = contextType
                    .GetProperties()
                    .Where(CheckPropertyToDbSet);

            return enumerable;
        }

        internal static IEnumerable<Type> GetContextEntities(Type contextType)
        {
            return GetDbSetProperties(contextType).Select(pi => pi.PropertyType.GetGenericArguments()[0]);
        }

        private static bool CheckPropertyToDbSet(PropertyInfo pi)
        {
            var checkPropertyToDbSet = pi.PropertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDbSet<>));
            return checkPropertyToDbSet;
        }

        private static ContextInfoCollection _contextInfoCollection;

        /// <summary>
        /// Коллекция объектов информаторов контекстов
        /// </summary>
        public static ContextInfoCollection ContextInfoCollection
        {
            get { return _contextInfoCollection ?? (_contextInfoCollection = new ContextInfoCollection()); }
        }
    }
}